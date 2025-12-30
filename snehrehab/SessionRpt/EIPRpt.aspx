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
