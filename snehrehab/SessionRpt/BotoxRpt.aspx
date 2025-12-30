<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="BotoxRpt.aspx.cs" Inherits="snehrehab.SessionRpt.BotoxRpt" Title="" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.checkboes{margin: 0px;max-height: 86px;overflow-x: auto;border: 1px solid #cccccc;padding: 5px;}
.checkboes input[type="checkbox"]{float: none;margin: 0px;}
.checkboes label{float: none;padding: 0px;width: auto;height: auto;display: inline-block;margin: 0px;line-height: normal;margin-bottom: 5px;margin-left: 10px;padding-top: 4px;}
.botox-default-table{}
.botox-default-table tr td{border: 1px solid #ccc;padding: 5px;}
.text100{width: 100% !important;max-width: 100px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Botox Report :</div>
            <div class="pull-right">
            <a href="/SessionRpt/BotoxView.aspx" class="btn btn-primary">View List</a>
            </div>
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
                </div>
            </div>
            <div class="clearfix">
            </div>
            <div class="formRow">
                <div class="span12">
                    <ajaxToolkit:TabContainer ID="tb_Contents" runat="server">
                        <ajaxToolkit:TabPanel ID="tb_Report1" runat="server" HeaderText="General Information">
                            <ContentTemplate>
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
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Botox No :
                                            </div>
                                           <div class="control-group">
                                                <asp:TextBox ID="General_BotoxNo" runat="server" CssClass="span5"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Pediatrician :
                                            </div>
                                           <div class="control-group">
                                                <asp:DropDownList ID="General_Pediatrician" runat="server" CssClass="chzn-select span5">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Therapist :
                                            </div>
                                           <div class="control-group">
                                                <asp:DropDownList ID="General_Therapist" runat="server" CssClass="chzn-select span5">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    
                                    
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tb_Report2" runat="server" HeaderText="History And Exam">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Delivery :
                                            </label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="HistoryExam_Delivery" runat="server" CssClass="chzn-select span3">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <label class="control-label">
                                                Birth Weight :
                                            </label>
                                            <div class="control-group">
                                                <asp:TextBox ID="HistoryExam_BirthWeight" runat="server" CssClass="span3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Perinatal Complications :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="HistoryExam_PerinatalComplications" runat="server" CssClass="span5" TextMode="MultiLine"
                                                    Rows="4"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div class="control-label">
                                                Milestones :
                                            </div>
                                            <div class="control-group">
                                                <div class="span5 checkboes" style="margin: 0px;">
                                                    <asp:CheckBoxList ID="MilestonesType" RepeatLayout="Flow" runat="server">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Diagnosed By :
                                            </label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="HistoryExam_DiagnosedBy" runat="server" CssClass="chzn-select span3">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tb_Report3" runat="server" HeaderText="Type Of CP">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Type Of CP :
                                            </label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="TypeOfCP_CPID" runat="server" CssClass="chzn-select span4">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tb_Report4" runat="server" HeaderText="Assistive Devices">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Assistive Devices and Mobility Aids/Orthotics : Left/Right/Bilateral :
                                            </div>
                                            <div class="span5 checkboes" style="margin: 0px;">
                                                <asp:CheckBoxList ID="AssistiveDevices" RepeatLayout="Flow" runat="server">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Orthotics :
                                            </div>
                                           <div class="control-group">
                                                <asp:DropDownList ID="AssistiveDevices_Orthotics" runat="server" CssClass="chzn-select span5">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                
                                            </div>
                                            <div class="span5 checkboes" style="margin: 0px;">
                                                <asp:CheckBoxList ID="OrthoticsDevices" RepeatLayout="Flow" runat="server">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tb_Report5" runat="server" HeaderText="ADL">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                ADL :
                                            </div>
                                           <div class="control-group">
                                                <asp:DropDownList ID="ADL_adlID" runat="server" CssClass="chzn-select span5">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                
                                            </div>
                                            <div class="span5 checkboes" style="margin: 0px;">
                                                <asp:CheckBoxList ID="ADLList" RepeatLayout="Flow" runat="server">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tb_Report6" runat="server" HeaderText="Ambulation">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="botox-default-table">
                                                <tr>
                                                    <td>
                                                        <b>DATE</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Date1" runat="server" Width="140px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Date2" runat="server" Width="140px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Date3" runat="server" Width="140px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Date4" runat="server" Width="140px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Date5" runat="server" Width="140px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Date6" runat="server" Width="140px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>AMBULATION</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Amb1" runat="server" Width="140px" CssClass="span1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Amb2" runat="server" Width="140px" CssClass="span1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Amb3" runat="server" Width="140px" CssClass="span1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Amb4" runat="server" Width="140px" CssClass="span1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Amb5" runat="server" Width="140px" CssClass="span1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Ambulation_Amb6" runat="server" Width="140px" CssClass="span1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br /><br />
                                            <ul>
                                                <li>Independent Community Ambulatory, no devices or wheelchair.</li>
                                                <li>Independent household ambulatory community ambulatory with device or w/c<50% or
                                                    time.</li>
                                                <li>Household and limited community ambulatory uses w/c>50% of time.</li>
                                                <li>Household or exercise ambulatory,all mobility with walker or w/c.</li>
                                                <li>Primary wheelchair user performs assisted weight bearing transfers.</li>
                                                <li>Primary wheelchair user requires dependent non-weight bearing transfers.</li>
                                            </ul>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tb_Report7" runat="server" HeaderText="Pre-existing Deformity">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="botox-default-table">
                                                <tr>
                                                    <td class="span2">
                                                        <b>DATE</b>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="PreExisting_Date1" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="PreExisting_Date2" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="PreExisting_Date3" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="PreExisting_Date4" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hip FID</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipFID_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipFID_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipFID_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipFID_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipFID_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipFID_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipFID_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipFID_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hip Adduction</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipAdduction_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipAdduction_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipAdduction_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipAdduction_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipAdduction_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipAdduction_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipAdduction_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_HipAdduction_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Knee FFD</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_KneeFFD_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_KneeFFD_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_KneeFFD_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_KneeFFD_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_KneeFFD_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_KneeFFD_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_KneeFFD_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_KneeFFD_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Equinus</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Equinus_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Equinus_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Equinus_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Equinus_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Equinus_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Equinus_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Equinus_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Equinus_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Planovalgoid</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Planovalgoid_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Planovalgoid_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Planovalgoid_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Planovalgoid_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Planovalgoid_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Planovalgoid_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Planovalgoid_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Planovalgoid_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Cavovarus</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Cavovarus_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Cavovarus_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Cavovarus_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Cavovarus_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Cavovarus_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Cavovarus_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Cavovarus_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_Cavovarus_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Elbow FFD</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_ElbowFFD_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_ElbowFFD_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_ElbowFFD_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_ElbowFFD_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_ElbowFFD_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_ElbowFFD_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_ElbowFFD_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_ElbowFFD_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Wrist Flex-Pron</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_WristFlexPron_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_WristFlexPron_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_WristFlexPron_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_WristFlexPron_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_WristFlexPron_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_WristFlexPron_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_WristFlexPron_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PreExisting_WristFlexPron_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
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
                        <ajaxToolkit:TabPanel ID="tb_Report8" runat="server" HeaderText="Passive ROM">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="botox-default-table">
                                                <tr>
                                                    <td class="span2">
                                                        <b>DATE</b>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="PassiveROM_Date1" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="PassiveROM_Date2" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="PassiveROM_Date3" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="PassiveROM_Date4" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hip Flexion</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipFlexion_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipFlexion_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipFlexion_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipFlexion_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipFlexion_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipFlexion_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipFlexion_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipFlexion_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hip Abduction</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipAbduction_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipAbduction_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipAbduction_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipAbduction_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipAbduction_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipAbduction_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipAbduction_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipAbduction_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hip IR</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipIR_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipIR_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipIR_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipIR_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipIR_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipIR_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipIR_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipIR_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hip ER</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipER_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipER_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipER_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipER_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipER_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipER_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipER_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_HipER_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Knee Flexion</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlexion_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlexion_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlexion_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlexion_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlexion_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlexion_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlexion_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlexion_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Popliteal Angle</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_PoplitealAngle_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_PoplitealAngle_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_PoplitealAngle_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_PoplitealAngle_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_PoplitealAngle_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_PoplitealAngle_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_PoplitealAngle_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_PoplitealAngle_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Dorsi(Knee ext)</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeExt_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeExt_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeExt_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeExt_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeExt_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeExt_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeExt_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeExt_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Dorsi(Knee flex)</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlex_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlex_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlex_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlex_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlex_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlex_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlex_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_KneeFlex_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Plantarflexion</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_Plantarflexion_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_Plantarflexion_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_Plantarflexion_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_Plantarflexion_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_Plantarflexion_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_Plantarflexion_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_Plantarflexion_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_Plantarflexion_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Ankle Inv</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleInv_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleInv_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleInv_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleInv_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleInv_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleInv_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleInv_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleInv_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Ankle Ever</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleEver_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleEver_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleEver_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleEver_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleEver_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleEver_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleEver_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PassiveROM_AnkleEver_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
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
                        <ajaxToolkit:TabPanel ID="tb_Report9" runat="server" HeaderText="Tone">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="botox-default-table">
                                                <tr>
                                                    <td class="span2">
                                                        <b>DATE</b>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="Tone_Date1" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="Tone_Date2" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="Tone_Date3" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="Tone_Date4" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Iliopsoas</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Iliopsoas_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Iliopsoas_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Iliopsoas_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Iliopsoas_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Iliopsoas_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Iliopsoas_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Iliopsoas_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Iliopsoas_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Adductors</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Adductors_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Adductors_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Adductors_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Adductors_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Adductors_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Adductors_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Adductors_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Adductors_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Rectus Femoris</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_RectusFemoris_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_RectusFemoris_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_RectusFemoris_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_RectusFemoris_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_RectusFemoris_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_RectusFemoris_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_RectusFemoris_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_RectusFemoris_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hamstrings</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Hamstrings_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Hamstrings_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Hamstrings_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Hamstrings_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Hamstrings_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Hamstrings_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Hamstrings_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Hamstrings_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gastrosoleus</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Gastrosoleus_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Gastrosoleus_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Gastrosoleus_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Gastrosoleus_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Gastrosoleus_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Gastrosoleus_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Gastrosoleus_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_Gastrosoleus_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Elbow Flexors</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_ElbowFlexors_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_ElbowFlexors_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_ElbowFlexors_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_ElbowFlexors_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_ElbowFlexors_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_ElbowFlexors_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_ElbowFlexors_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_ElbowFlexors_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Wrist Flexors</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_WristFlexors_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_WristFlexors_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_WristFlexors_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_WristFlexors_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_WristFlexors_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_WristFlexors_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_WristFlexors_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_WristFlexors_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Finger Flexors</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_FingerFlexors_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_FingerFlexors_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_FingerFlexors_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_FingerFlexors_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_FingerFlexors_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_FingerFlexors_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_FingerFlexors_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_FingerFlexors_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Pronator Flexors</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_PronatorFlexors_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_PronatorFlexors_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_PronatorFlexors_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_PronatorFlexors_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_PronatorFlexors_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_PronatorFlexors_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_PronatorFlexors_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Tone_PronatorFlexors_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
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
                        <ajaxToolkit:TabPanel ID="tb_Report10" runat="server" HeaderText="Tardieus Scale">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="botox-default-table">
                                                <tr>
                                                    <td class="span2">
                                                        <b>DATE</b>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="TardieusScale_Date1" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="TardieusScale_Date2" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="TardieusScale_Date3" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="TardieusScale_Date4" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gastrosoleus R1</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR1_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR1_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR1_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR1_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR1_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR1_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR1_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR1_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gastrosoleus R2</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR2_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR2_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR2_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR2_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR2_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR2_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR2_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR2_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gastrosoleus R3</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR3_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR3_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR3_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR3_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR3_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR3_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR3_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_GastrosoleusR3_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hamstrings R1</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR1_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR1_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR1_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR1_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR1_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR1_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR1_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR1_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hamstrings R2</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR2_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR2_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR2_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR2_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR2_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR2_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR2_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TardieusScale_HamstringsR2_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
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
                        <ajaxToolkit:TabPanel ID="tb_Report11" runat="server" HeaderText="Muscle Strength">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="botox-default-table">
                                                <tr>
                                                    <td class="span2">
                                                        <b>DATE</b>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="MuscleStrength_Date1" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="MuscleStrength_Date2" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="MuscleStrength_Date3" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="MuscleStrength_Date4" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Iliopsoas</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Iliopsoas_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Iliopsoas_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Iliopsoas_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Iliopsoas_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Iliopsoas_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Iliopsoas_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Iliopsoas_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Iliopsoas_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gluteus Max</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_GluteusMax_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_GluteusMax_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_GluteusMax_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_GluteusMax_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_GluteusMax_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_GluteusMax_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_GluteusMax_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_GluteusMax_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Abductors</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Abductors_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Abductors_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Abductors_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Abductors_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Abductors_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Abductors_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Abductors_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Abductors_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Rectus Femoris</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_RectusFemoris_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_RectusFemoris_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_RectusFemoris_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_RectusFemoris_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_RectusFemoris_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_RectusFemoris_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_RectusFemoris_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_RectusFemoris_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hamstrings</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Hamstrings_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Hamstrings_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Hamstrings_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Hamstrings_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Hamstrings_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Hamstrings_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Hamstrings_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Hamstrings_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gastrosoleus</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Gastrosoleus_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Gastrosoleus_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Gastrosoleus_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Gastrosoleus_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Gastrosoleus_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Gastrosoleus_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Gastrosoleus_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_Gastrosoleus_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Tibialis Ant</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_TibialisAnt_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_TibialisAnt_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_TibialisAnt_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_TibialisAnt_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_TibialisAnt_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_TibialisAnt_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_TibialisAnt_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_TibialisAnt_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Elbow Flexors</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_ElbowFlexors_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_ElbowFlexors_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_ElbowFlexors_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_ElbowFlexors_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_ElbowFlexors_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_ElbowFlexors_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_ElbowFlexors_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_ElbowFlexors_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Pronator Teres</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_PronatorTeres_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_PronatorTeres_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_PronatorTeres_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_PronatorTeres_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_PronatorTeres_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_PronatorTeres_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_PronatorTeres_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_PronatorTeres_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Wrist Flexors</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristFlexors_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristFlexors_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristFlexors_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristFlexors_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristFlexors_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristFlexors_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristFlexors_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristFlexors_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Wrist Extensors</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristExtensors_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristExtensors_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristExtensors_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristExtensors_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristExtensors_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristExtensors_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristExtensors_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_WristExtensors_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Finger Flexors</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_FingerFlexors_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_FingerFlexors_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_FingerFlexors_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_FingerFlexors_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_FingerFlexors_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_FingerFlexors_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_FingerFlexors_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MuscleStrength_FingerFlexors_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
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
                        <ajaxToolkit:TabPanel ID="tb_Report12" runat="server" HeaderText="Voluntary Control">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="botox-default-table">
                                                <tr>
                                                    <td class="span2">
                                                        <b>DATE</b>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="Voluntary_Date1" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="Voluntary_Date2" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="Voluntary_Date3" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="Voluntary_Date4" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                    <td>
                                                        <b>R</b>
                                                    </td>
                                                    <td>
                                                        <b>L</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hip Flexion</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipFlexion_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipFlexion_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipFlexion_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipFlexion_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipFlexion_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipFlexion_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipFlexion_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipFlexion_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hip Extension</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipExtension_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipExtension_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipExtension_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipExtension_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipExtension_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipExtension_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipExtension_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipExtension_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Hip Abduction</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipAbduction_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipAbduction_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipAbduction_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipAbduction_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipAbduction_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipAbduction_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipAbduction_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_HipAbduction_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Knee Flexion</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeFlexion_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeFlexion_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeFlexion_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeFlexion_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeFlexion_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeFlexion_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeFlexion_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeFlexion_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Knee Extension</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeExtension_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeExtension_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeExtension_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeExtension_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeExtension_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeExtension_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeExtension_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_KneeExtension_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Dorsiflexion</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Dorsiflexion_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Dorsiflexion_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Dorsiflexion_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Dorsiflexion_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Dorsiflexion_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Dorsiflexion_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Dorsiflexion_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Dorsiflexion_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Plantarflexion</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Plantarflexion_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Plantarflexion_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Plantarflexion_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Plantarflexion_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Plantarflexion_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Plantarflexion_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Plantarflexion_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Plantarflexion_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Wrist Dorsiflex</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_WristDorsiflex_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_WristDorsiflex_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_WristDorsiflex_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_WristDorsiflex_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_WristDorsiflex_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_WristDorsiflex_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_WristDorsiflex_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_WristDorsiflex_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Grasp</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Grasp_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Grasp_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Grasp_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Grasp_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Grasp_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Grasp_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Grasp_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Grasp_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Release</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Release_1R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Release_1L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Release_2R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Release_2L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Release_3R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Release_3L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Release_4R" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Voluntary_Release_4L" runat="server" CssClass="span1 text100"></asp:TextBox>
                                                    </td>
                                                </tr>
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
                        <ajaxToolkit:TabPanel ID="tb_Report13" runat="server" HeaderText="Functional Strength">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="botox-default-table">
                                                <tr>
                                                    <td class="span2">
                                                        <b>DATE</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Date1" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td >
                                                        <asp:TextBox ID="FunctionalStrength_Date2" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Date3" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Date4" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Pull to Stand</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_PullStand_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_PullStand_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_PullStand_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_PullStand_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Independent Standing Arms Free(3 sec)</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_Independent3Sec_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_Independent3Sec_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Independent3Sec_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Independent3Sec_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Independent Standing Arms Free(20 sec)</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_Independent20Sec_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_Independent20Sec_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Independent20Sec_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Independent20Sec_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>One Leg Stance Hand Held : R</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_HandHeldR_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_HandHeldR_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_HandHeldR_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_HandHeldR_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>One Leg Stance Hand Held : L</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_HandHeldL_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_HandHeldL_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_HandHeldL_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_HandHeldL_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>One Leg Stance : R</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_OneLegR_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_OneLegR_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_OneLegR_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_OneLegR_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>One Leg Stance : L</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_OneLegL_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_OneLegL_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_OneLegL_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_OneLegL_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Short Sit to Stand</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_ShortSit_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_ShortSit_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_ShortSit_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_ShortSit_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>High Knee to Stand : R</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_HighKneeR_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_HighKneeR_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_HighKneeR_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_HighKneeR_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>High Knee to Stand : L</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_HighKneeL_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_HighKneeL_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_HighKneeL_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_HighKneeL_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Lowers to Floor</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_LowersFloor_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_LowersFloor_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_LowersFloor_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_LowersFloor_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Squats</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_Squats_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_Squats_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Squats_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Squats_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Standing-Picks Pen from Floor</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_StandingPicks_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_StandingPicks_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_StandingPicks_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_StandingPicks_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Total(39)</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_Total_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="FunctionalStrength_Total_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Total_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="FunctionalStrength_Total_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
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
                        <ajaxToolkit:TabPanel ID="tb_Report14" runat="server" HeaderText="Indication For Botox">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Indication For Botox :
                                            </div>
                                            <div class="span5 checkboes" style="margin: 0px;">
                                                <asp:CheckBoxList ID="IndicationForBotox" RepeatLayout="Flow" runat="server">
                                                </asp:CheckBoxList>
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
                        <ajaxToolkit:TabPanel ID="tb_Report15" runat="server" HeaderText="Botox Data">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="botox-default-table">
                                                <tr>
                                                    <td class="span2">
                                                        <b>DATE</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Date1" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td >
                                                        <asp:TextBox ID="BotoxData_Date2" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Date3" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Date4" runat="server" Width="200px" CssClass="span1 my-datepicker"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>1: Weight</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Weight_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Weight_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Weight_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Weight_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>2: Total Does of Botox Injected(Units)</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_BotoxInjected_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_BotoxInjected_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_BotoxInjected_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_BotoxInjected_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>3: Dilution(ml) in 0.9% NS</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Dilution_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Dilution_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Dilution_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Dilution_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>4: Muscles Injected(vol)</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_MusclesInjected_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_MusclesInjected_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_MusclesInjected_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_MusclesInjected_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 Gastocnemius-Soleus</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Gastocnemius_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Gastocnemius_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Gastocnemius_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Gastocnemius_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 Tibialis Posterior</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Tibialis_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Tibialis_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Tibialis_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Tibialis_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 Hamstrings</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Hamstrings_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Hamstrings_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Hamstrings_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Hamstrings_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 Adductors</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Adductors_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Adductors_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Adductors_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Adductors_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 Rectus</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Rectus_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Rectus_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Rectus_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Rectus_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 Iliopsoas</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Iliopsoas_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Iliopsoas_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Iliopsoas_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Iliopsoas_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 Pronator Teres</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Pronator_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Pronator_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Pronator_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Pronator_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 FCR</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_FCR_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_FCR_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_FCR_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_FCR_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 FCU</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_FCU_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_FCU_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_FCU_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_FCU_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 FDS</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_FDS_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_FDS_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_FDS_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_FDS_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 FDP</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_FDP_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_FDP_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_FDP_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_FDP_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 FPL</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_FPL_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_FPL_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_FPL_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_FPL_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 Adductor</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Adductor_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Adductor_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Adductor_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Adductor_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>---0 Intrinsics</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Intrinsics_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Intrinsics_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Intrinsics_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Intrinsics_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>5: Casting</b>
                                                    </td>
                                                    
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Casting_1" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                       <asp:TextBox ID="BotoxData_Casting_2" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Casting_3" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                   
                                                    <td>
                                                        <asp:TextBox ID="BotoxData_Casting_4" runat="server" Width="200px" CssClass="span1 "></asp:TextBox>
                                                    </td>
                                                </tr>
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
                        <ajaxToolkit:TabPanel ID="tb_Report16" runat="server" HeaderText="Ancillary Treatment">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Ancillary Treatment :
                                            </div>
                                            <div class="span5 checkboes" style="margin: 0px;">
                                                <asp:CheckBoxList ID="AncillaryTreatment" RepeatLayout="Flow" runat="server">
                                                </asp:CheckBoxList>
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
                        <ajaxToolkit:TabPanel ID="tb_Report17" runat="server" HeaderText="Side Effects">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                   <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Side Effects :
                                            </div>
                                            <div class="span5 checkboes" style="margin: 0px;">
                                                <asp:CheckBoxList ID="SideEffects" RepeatLayout="Flow" runat="server">
                                                </asp:CheckBoxList>
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
                        <ajaxToolkit:TabPanel ID="tb_Report18" runat="server" HeaderText="Doctor">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Director :
                                            </div>
                                           <div class="control-group">
                                                <asp:DropDownList ID="Doctor_Director" runat="server" CssClass="chzn-select span5">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div><div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Physiotheraist :
                                            </div>
                                           <div class="control-group">
                                                <asp:DropDownList ID="Doctor_Physiotheraist" runat="server" CssClass="chzn-select span5">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div><div class="formRow">
                                        <div class="span6">
                                            <div class="control-label">
                                                Occupational Therapist :
                                            </div>
                                           <div class="control-group">
                                                <asp:DropDownList ID="Doctor_Occupational" runat="server" CssClass="chzn-select span5">
                                                </asp:DropDownList>
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
                    </ajaxToolkit:TabContainer>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>

      <script>
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
      </script>
</asp:Content>
