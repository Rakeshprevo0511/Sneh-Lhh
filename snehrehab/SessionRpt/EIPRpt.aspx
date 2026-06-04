<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="EIPRpt.aspx.cs" Inherits="snehrehab.SessionRpt.EIPRpt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Morphology-OuterTopTable
        {
        }
        .Morphology-OuterTopTable tr td
        {
            padding: 5px;
            border: 1px solid #ccc;
            text-align: center;
        }
        .Morphology-Upper-Limb
        {
        }
        .Morphology-Upper-Limb tr td
        {
            padding: 5px;
            border: 1px solid #CCC;
        }
        .Morphology-Lower-Limb
        {
        }
        .Morphology-Lower-Limb tr td
        {
            padding: 5px;
            border: 1px solid #CCC;
        }
        .ndt-default-table
        {
        }
        .ndt-default-table tr td
        {
            border: 1px solid #ccc;
            padding: 10px;
        }
        span.char-limit-msg
        {
            font-style: italic;
            color: red;
            font-size: 11px;
        }
        .textbox
        {
            margin-bottom: 5px !important;
            width: 256px !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var maxLines = 8; var maxChar = 400;
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
                EIA Report:
            </div>
            <div class="pull-right">
                <a href="/SessionRpt/EIPView.aspx" class="btn btn-primary">View List</a>
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
            <ajaxtoolkit:tabcontainer id="tb_Contents" runat="server">
            <div class="formRow">
                <div class="span12"> 
                    <ajaxToolkit:TabPanel ID="tb_Report1" runat="server" HeaderText="Data Collection">
                    <ContentTemplate>                                                                                                                                                                                                               <div style="margin-top: 20px; margin-bottom: 20px;">
                     <div class="formRow ">
                        <div class="span12">
                          <div class="control-label">
                           1. EDD :
                          </div>
                          <div class="control-group">
                           <asp:TextBox ID="DataCollection_EDD" runat="server" CssClass="span10" TextMode="MultiLine" Rows="3"></asp:TextBox>
                          </div>
                          </div>
                          <div class="clearfix">
                          </div>
                      </div>
                       <div class="formRow ">
                        <div class="span12">
                          <div class="control-label">
                           2. CGA :
                          </div>
                          <div class="control-group">
                           <asp:TextBox ID="DataCollection_CGA" runat="server" CssClass="span10" TextMode="MultiLine" Rows="3"></asp:TextBox>
                          </div>
                          </div>
                          <div class="clearfix">
                          </div>
                      </div>
                      </div>
                    </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                
                    <ajaxToolkit:TabPanel ID="tb_Report2" runat="server" HeaderText="Birth History">
                    <ContentTemplate>                                                                                                                                                                                                                                                                                                                                                                                                                                                                <div style="margin-top: 20px; margin-bottom: 20px;">
                     <div class="formRow ">
                        <div class="span12">
                          <div class="control-label">
                           1. N/C SEC DELIVERY :
                          </div>
                          <div class="control-group">
                           <asp:TextBox ID="txtbirthhistory_nc" runat="server" CssClass="span10"></asp:TextBox>
                          </div>
                          </div>
                          <div class="clearfix">
                          </div>
                      </div>
                       <div class="formRow char-line-limiter">
                        <div class="span12">
                          <div class="control-label">
                           2. PRE – NATAL / MATERAIL HISTORY  :
                          </div>
                          <div class="control-group">
                           <asp:TextBox ID="txtbirthhistory_prenatal" runat="server" CssClass="span10" TextMode="MultiLine" Rows="4"></asp:TextBox>
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
                           3. NATAL :
                          </div>
                          <div class="control-group">
                           <asp:TextBox ID="txtbirthhistorynatal" runat="server" CssClass="span10" TextMode="MultiLine" Rows="4"></asp:TextBox>
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
                           4. POST NATAL H/O :
                          </div>
                          <div class="control-group">
                           <asp:TextBox ID="txtbirthhistory_postnatal" runat="server" CssClass="span10" TextMode="MultiLine" Rows="4"></asp:TextBox>
                          </div>
                          <div class="clearfix"></div>
                          <span class="char-limit-msg"></span>
                          </div>
                          <div class="clearfix">
                          </div>
                      </div>
                    </div>
                    </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tb_Report3" runat="server" HeaderText="Observation">
                    <ContentTemplate>
                        <div class="span12">
                            <h5>a) Autonomic :</h5>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    1) HR :
                                </div>
                                <div class="control-group span8">
                                    <asp:TextBox ID="txtobservationhr" runat="server" CssClass="span8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    2) TYPES OF RESPIRATION :
                                </div>
                                <div class="control-group span8">
                                    <asp:TextBox ID="txtobservationrespiration" runat="server" CssClass="span8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    3) SKIN COLOUR :
                                </div>
                                <div class="control-group span8">
                                    <asp:TextBox ID="txtobservationskincolor" runat="server" CssClass="span8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    4) TEMPERATURE –CENTRAL /PERIPHERAL :
                                </div>
                                <div class="control-group span8">
                                    <asp:TextBox ID="txtobservationtemperature" runat="server" CssClass="span8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    <h5>b) Motor :</h5>
                                </div>
                                <div class="control-group span8">
                                    <asp:TextBox ID="txtobservationMotor" runat="server" CssClass="span8" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                       <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    Upper Limb :
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                         <div class="formRow ">
                            <div class="span12">
                                <table class="OBSERVATION-Upper-Limb span10">
                                    <tr>
                                        <td class="span2">
                                            Level
                                        </td>
                                        <td class="span2">
                                             Left
                                        </td>
                                        <td class="span2">
                                            Right
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtobservationUpperLimbLevel1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationUpperLimbLeft1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationUpperLimbRight1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                
                                    </tr>
                                    <tr>
                                
                                        <td>
                                            <asp:TextBox ID="txtobservationUpperLimbLevel2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationUpperLimbLeft2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationUpperLimbRight2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtobservationUpperLimbLevel3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationUpperLimbLeft3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationUpperLimbRight3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    Lower Limb :
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                         <div class="formRow ">
                            <div class="span12">
                                <table class="OBSERVATION-Upper-Limb span10">
                                    <tr>
                                        <td class="span2">
                                            Level
                                        </td>
                                        <td class="span2">
                                             Left
                                        </td>
                                        <td class="span2">
                                            Right
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtobservationLowerLimbLevel1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationLowerLimbLeft1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationLowerLimbRight1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                
                                    </tr>
                                    <tr>
                                
                                        <td>
                                            <asp:TextBox ID="txtobservationLowerLimbLevel2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationLowerLimbLeft2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationLowerLimbRight2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtobservationLowerLimbLevel3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationLowerLimbLeft3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtobservationLowerLimbRight3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    Trunk  :
                                </div>
                                <div class="span8">
                                    <asp:TextBox ID="txtobservationtrunk" runat="server" CssClass="span8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    General Posture  :</div>
                                <div class="span8">
                                    <asp:TextBox ID="txtobservationgeneralposture" runat="server" CssClass="span8" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    <h5>c) Social Interaction /Responsivity :</h5></div>
                                <div class="span8">
                                    <asp:TextBox ID="txtobservationsocialinteraction" runat="server" CssClass="span8" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    <h5>d) Feeding :</h5></div>
                                <div class="span8">
                                    <asp:TextBox ID="txtobservationFeeding" runat="server" CssClass="span8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                    a) Participation :</div>
                                <div class="span8">
                                    <asp:TextBox ID="txtobservationParticipation" runat="server" CssClass="span8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow ">
                            <div class="span11">
                                <div class="control-label span2">
                                   b) Participation Restriction :</div>
                                <div class="span8">
                                    <asp:TextBox ID="txtobservationParticipationRestriction" runat="server" CssClass="span8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div> 
                    </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tb_Report4" runat="server" HeaderText="Examination">
                    <ContentTemplate>
                        <div class="span12">
                            <h5>Examination :</h5>
                        </div>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="form-row">
                                <div class="span12">
                                    <h5> Ballards :</h5>
                                    <table class="Examination-Ballards span10">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards4" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards5" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards6" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards7" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards8" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards9" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards10" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards11" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExaminationBallards12" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="form-row">
                                    <div class="span12">
                                        <h5>Timp :</h5>
                                        <table class="Examination-Timp span10">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtExaminationTimp1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExaminationTimp2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtExaminationTimp3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExaminationTimp4" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtExaminationTimp5" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExaminationTimp6" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtExaminationTimp7" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExaminationTimp8" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                </td>

                                            </tr>
                                        </table>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                    <div class="form-row">
                                        <div class="span12">
                                            <h5> Voitas :</h5>
                                            <table class="Examination-Voitas span10">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtExaminationVoitas1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtExaminationVoitas2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtExaminationVoitas3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtExaminationVoitas4" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtExaminationVoitas5" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtExaminationVoitas6" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtExaminationVoitas7" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtExaminationVoitas8" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                                    </td>

                                                </tr>
                                            </table>
                                        </div>
                                        <div class="clearfix"> </div>
                                        <div class="formRow ">
                                            <div class="span11">
                                                <div class="control-label span2">
                                                    <h5>Goals Of Treatment :</h5></div>
                                                <div class="span8">
                                                    <asp:TextBox ID="txtExaminationgoalstreatment" runat="server" TextMode="MultiLine" Rows="2" CssClass="span8"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                        <div class="formRow ">
                                            <div class="span11">
                                                <div class="control-label span2">
                                                    <h5>Treatment Given :</h5></div>
                                                <div class="span8">
                                                    <asp:TextBox ID="txtExaminationtreatmentgiven" runat="server" TextMode="MultiLine" Rows="2" CssClass="span8"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                        </div>
                        </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                      <ajaxToolkit:TabPanel ID="tb_Report5" runat="server" HeaderText="Doctor">
                    <ContentTemplate>
                       
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="form-row">
                                <div class="form-row">
                                    <div class="form-row">
                                        <div class="clearfix"> </div>
                                        <div class="formRow ">
                                            <div class="span11">
                                                <div class="control-label span2">
                                                    1. Physioptherapist :</div>
                                                <div class="span8">
                                                    <asp:DropDownList ID="Doctor_Physioptherapist" runat="server" CssClass="chzn-select span6"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                        <div class="formRow ">
                                            <div class="span11">
                                                <div class="control-label span2">
                                                    2. Physioptherapist  : </div>
                                                <div class="span8">
                                                    <asp:DropDownList ID="Doctor_Occupational" runat="server" CssClass="chzn-select span6"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                        <div class="formRow " style="visibility:hidden">
                                            <div class="span11">
                                                <div class="control-label span2">
                                                    3. Name of Director :</div>
                                                <div class="span8">
                                                    <asp:DropDownList ID="Doctor_EnterReport" runat="server" CssClass="chzn-select span6"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    </ajaxtoolkit:tabcontainer>
                 </div>
                <div class="clearfix">
                </div>
            </div>
     <asp:HiddenField ID="Hdnrange2" runat="server" /> <asp:HiddenField ID="hdnrange" runat="server" />

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

             formData.ClinicalObsevation = $("#<%= DataCollection_EDD.ClientID %>").val();

             var list = [];


             formData.TimelineJson = JSON.stringify(list);

             saveWithModal(formData, reloadAfterSave, "CLINICAL_OBSERVATION AND DAILY SCHEDULE ");
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
        
     </script>
</asp:Content>
