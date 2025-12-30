<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="DailyRpt1.aspx.cs" Inherits="snehrehab.SessionRpt.DailyRpt1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.checkboes{margin: 0px;max-height: 86px;overflow-x: auto;border: 1px solid #cccccc;padding: 5px;}
.checkboes input[type="checkbox"]{float: none;margin: 0px;}
.checkboes label{float: none;padding: 0px;width: auto;height: auto;display: inline-block;margin: 0px;line-height: normal;margin-bottom: 5px;margin-left: 10px;padding-top: 4px;}
.checkboes{float:left;margin-right:10px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Daily Report :</div>
            <div class="pull-right">
            </div>
        </div>
        <div class="grid-content" style="padding: 0px;">
            <ajaxToolkit:TabContainer ID="tb_Contents" runat="server" CssClass="fancy fancy-green">
                <ajaxToolkit:TabPanel ID="tb_Report1" runat="server" HeaderText="PART - 1">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
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
                            <asp:Panel ID="PanelDiagnosis" runat="server">
                                <div class="formRow">
                                    <div class="span6">
                                        <label class="control-label">Diagnosis :</label>
                                        <div class="control-group">
                                            <asp:ListBox ID="txtDiagnosis" runat="server" SelectionMode="Multiple" CssClass="chzn-select-multi span4" data-placeholder="Select Diagnosis"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <label class="control-label">Other Diagnosis :</label>
                                        <div class="control-group">
                                            <asp:TextBox ID="txtDiagnosisOther" runat="server" CssClass="span4"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </asp:Panel>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Observation :</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtObservation" runat="server" CssClass="span4" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Suggestion :</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtSuggestion" runat="server" CssClass="span4" TextMode="MultiLine" Rows="4"></asp:TextBox>
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
                                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-danger" Text=" Submit " OnClientClick="DisableOnSubmit(this);" onclick="btnSubmit_Click"></asp:LinkButton>
                                        &nbsp;
                                        <%= _printUrl %>
                                        <a href='<%= _cancelUrl %>' class="btn btn-default">Cancel</a>
                                        <asp:HiddenField ID="txtPrint" runat="server" />
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
    </div>
</asp:Content>
