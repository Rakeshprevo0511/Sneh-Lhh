<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Demo_PreScreenRpt.aspx.cs" Inherits="snehrehab.SessionRpt.Demo_PreScreenRpt" %>
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
                Pre Screening Report :</div>
            <%--<div class="pull-right">
            <a href="/SessionRpt/Demo_PreScreenView.aspx" class="btn btn-primary">View List</a>
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
                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-danger" Text=" Submit "
                            OnClientClick="DisableOnSubmit(this);" OnClick="btnSubmit_Click"></asp:LinkButton>
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
                        <ajaxToolkit:TabPanel ID="TabPanel12" runat="server" HeaderText="History">
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
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                <h6>
                                                    Please give us details about your family structure :</h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Family History :
                                            </div>
                                        </div>
                                        <div class="span9">
                                            <div class="control-label">
                                                <asp:CheckBox ID="His_FamilyHistory_1" runat="server" Enabled="false" onclick="His_FamilyHistory_1_Click();"
                                                    CssClass="checkboes" Text=" Consanguineous" />
                                                <asp:CheckBox ID="His_FamilyHistory_2" runat="server" Enabled="false" onclick="His_FamilyHistory_2_Click();"
                                                    CssClass="checkboes" Text=" Non-consanguineous marriage" />
                                                <script type="text/javascript">
                                                    function His_FamilyHistory_1_Click() {
                                                        var ctl = $('#<%=His_FamilyHistory_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_FamilyHistory_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function His_FamilyHistory_2_Click() {
                                                        var ctl = $('#<%=His_FamilyHistory_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_FamilyHistory_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Family Structure :
                                            </div>
                                        </div>
                                        <div class="span9">
                                            <div class="control-label">
                                                <asp:CheckBox ID="His_FamilyStru_1" runat="server" Enabled="false" onclick="His_FamilyStru_1_Click();"
                                                    CssClass="checkboes" Text=" Nuclear" />
                                                <asp:CheckBox ID="His_FamilyStru_2" runat="server" Enabled="false" onclick="His_FamilyStru_2_Click();"
                                                    CssClass="checkboes" Text=" Joint Family" />
                                                <script type="text/javascript">
                                                    function His_FamilyStru_1_Click() {
                                                        var ctl = $('#<%=His_FamilyStru_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_FamilyStru_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function His_FamilyStru_2_Click() {
                                                        var ctl = $('#<%=His_FamilyStru_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_FamilyStru_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                <h6>
                                                    Please tell us about your relationships with the child and amongst yourself ?</h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Family Relation :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Inter-parental relationship :
                                                        </div>
                                                        <asp:TextBox ID="His_InterParental" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Parent child relationship :
                                                        </div>
                                                        <asp:TextBox ID="His_ParentalChild" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span5">
                                            <div class="control-label">
                                                Domestic violence/physical or verbal abuse/emotional abuse :
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div class="control-label">
                                                <asp:CheckBox ID="His_EmotionalAbus_1" runat="server" Enabled="false" onclick="His_EmotionalAbus_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="His_EmotionalAbus_2" runat="server" Enabled="false" onclick="His_EmotionalAbus_2_Click();"
                                                    CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function His_EmotionalAbus_1_Click() {
                                                        var ctl = $('#<%=His_EmotionalAbus_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_EmotionalAbus_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function His_EmotionalAbus_2_Click() {
                                                        var ctl = $('#<%=His_EmotionalAbus_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_EmotionalAbus_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Family Relocation :
                                            </div>
                                        </div>
                                        <div class="span9">
                                            <div class="control-label">
                                                <asp:CheckBox ID="His_FamilyRelocation_1" runat="server" Enabled="false" onclick="His_FamilyRelocation_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="His_FamilyRelocation_2" runat="server" Enabled="false" onclick="His_FamilyRelocation_2_Click();"
                                                    CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function His_FamilyRelocation_1_Click() {
                                                        var ctl = $('#<%=His_FamilyRelocation_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_FamilyRelocation_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function His_FamilyRelocation_2_Click() {
                                                        var ctl = $('#<%=His_FamilyRelocation_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_FamilyRelocation_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Primary Care Givers :
                                            </div>
                                        </div>
                                        <div class="span9">
                                            <asp:TextBox ID="His_PrimaryCareGiver" runat="server" Enabled="false" CssClass="span9"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                <h6>
                                                    Maternal History</h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Maternal History :
                                            </div>
                                        </div>
                                        <div class="span3">
                                            <div class="control-label">
                                                <asp:CheckBox ID="His_MaternalHistory_1" runat="server" Enabled="false" onclick="His_MaternalHistory_1_Click();"
                                                    CssClass="checkboes" Text=" 1st Child" />
                                                <asp:CheckBox ID="His_MaternalHistory_2" runat="server" Enabled="false" onclick="His_MaternalHistory_2_Click();"
                                                    CssClass="checkboes" Text=" Siblings" />
                                                <script type="text/javascript">
                                                    function His_MaternalHistory_1_Click() {
                                                        var ctl = $('#<%=His_MaternalHistory_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_MaternalHistory_2.ClientID %>').prop('checked', false);
                                                            His_MaternalHistory_3_Check();
                                                        }
                                                    }
                                                    function His_MaternalHistory_2_Click() {
                                                        var ctl = $('#<%=His_MaternalHistory_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_MaternalHistory_1.ClientID %>').prop('checked', false);
                                                            His_MaternalHistory_3_Check();
                                                        }
                                                    }
                                                    $(function () {
                                                        His_MaternalHistory_3_Check();
                                                    });
                                                    function His_MaternalHistory_3_Check() {
                                                        var ctl = $('#<%=His_MaternalHistory_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_MaternalHistory_3.ClientID %>').removeAttr('disabled');
                                                        } else {
                                                            $('#<%=His_MaternalHistory_3.ClientID %>').val('');
                                                            $('#<%=His_MaternalHistory_3.ClientID %>').attr('disabled', 'disabled');
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="span1">
                                            How Many
                                        </div>
                                        <div class="span3">
                                            <asp:TextBox ID="His_MaternalHistory_3" runat="server" Enabled="false" class="span3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Any History Of :
                                            </div>
                                        </div>
                                        <div class="span4">
                                            <div class="control-label">
                                                <asp:CheckBox ID="His_AnyHistoryOf_1" runat="server" Enabled="false" onclick="His_AnyHistoryOf_1_Click();"
                                                    CssClass="checkboes" Text=" Diabetes" />
                                                <asp:CheckBox ID="His_AnyHistoryOf_2" runat="server" Enabled="false" onclick="His_AnyHistoryOf_2_Click();"
                                                    CssClass="checkboes" Text=" Hypertension" />
                                                <asp:CheckBox ID="His_AnyHistoryOf_3" runat="server" Enabled="false" onclick="His_AnyHistoryOf_3_Click();"
                                                    CssClass="checkboes" Text=" Others" />
                                                <script type="text/javascript">
                                                    function His_AnyHistoryOf_1_Click() {
                                                        var ctl = $('#<%=His_AnyHistoryOf_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_AnyHistoryOf_2.ClientID %>').prop('checked', false);
                                                            $('#<%=His_AnyHistoryOf_3.ClientID %>').prop('checked', false);
                                                            His_AnyHistoryOf_Check();
                                                        }
                                                    }
                                                    function His_AnyHistoryOf_2_Click() {
                                                        var ctl = $('#<%=His_AnyHistoryOf_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_AnyHistoryOf_1.ClientID %>').prop('checked', false);
                                                            $('#<%=His_AnyHistoryOf_3.ClientID %>').prop('checked', false);
                                                            His_AnyHistoryOf_Check();
                                                        }
                                                    }
                                                    function His_AnyHistoryOf_3_Click() {
                                                        var ctl = $('#<%=His_AnyHistoryOf_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_AnyHistoryOf_1.ClientID %>').prop('checked', false);
                                                            $('#<%=His_AnyHistoryOf_2.ClientID %>').prop('checked', false);
                                                            His_AnyHistoryOf_Check();
                                                        }
                                                    }
                                                    $(function () {
                                                        His_AnyHistoryOf_Check();
                                                    });
                                                    function His_AnyHistoryOf_Check() {
                                                        var ctl = $('#<%=His_AnyHistoryOf_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=His_AnyHistoryOf_4.ClientID %>').removeAttr('disabled');
                                                        } else {
                                                            $('#<%=His_AnyHistoryOf_4.ClientID %>').val('');
                                                            $('#<%=His_AnyHistoryOf_4.ClientID %>').attr('disabled', 'disabled');
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="span3">
                                            <asp:TextBox ID="His_AnyHistoryOf_4" runat="server" Enabled="false" class="span3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                <h6>
                                                    Pre-Natal History</h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                Any complications during pregnancy :
                                            </div>
                                        </div>
                                        <div class="span3">
                                            <div class="control-label">
                                                <asp:CheckBox ID="PreNatal_AnyComplication_1" runat="server" Enabled="false" onclick="PreNatal_AnyComplication_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="PreNatal_AnyComplication_2" runat="server" Enabled="false" onclick="PreNatal_AnyComplication_2_Click();"
                                                    CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function PreNatal_AnyComplication_1_Click() {
                                                        var ctl = $('#<%=PreNatal_AnyComplication_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PreNatal_AnyComplication_2.ClientID %>').prop('checked', false);
                                                            PreNatal_AnyComplication_Check();
                                                        }
                                                    }
                                                    function PreNatal_AnyComplication_2_Click() {
                                                        var ctl = $('#<%=PreNatal_AnyComplication_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PreNatal_AnyComplication_1.ClientID %>').prop('checked', false);
                                                            PreNatal_AnyComplication_Check();
                                                        }
                                                    }
                                                    $(function () {
                                                        PreNatal_AnyComplication_Check();
                                                    })
                                                    function PreNatal_AnyComplication_Check() {
                                                        var ctl = $('#<%=PreNatal_AnyComplication_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PreNatal_Complications.ClientID %>').removeAttr('disabled');
                                                        } else {
                                                            $('#<%=PreNatal_Complications.ClientID %>').val('');
                                                            $('#<%=PreNatal_Complications.ClientID %>').attr('disabled', 'disabled');
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="span3">
                                            <asp:TextBox ID="PreNatal_Complications" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel13" runat="server" HeaderText="Birth History">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Birth History :
                                            </div>
                                        </div>
                                        <div class="span3">
                                            <div class="control-label">
                                                <asp:CheckBox ID="BirthHis_Terms_1" runat="server" Enabled="false" onclick="BirthHis_Terms_1_Click();"
                                                    CssClass="checkboes" Text=" Full Term" />
                                                <asp:CheckBox ID="BirthHis_Terms_2" runat="server" Enabled="false" onclick="BirthHis_Terms_2_Click();"
                                                    CssClass="checkboes" Text=" Pre Term" />
                                                <script type="text/javascript">
                                                    function BirthHis_Terms_1_Click() {
                                                        var ctl = $('#<%=BirthHis_Terms_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BirthHis_Terms_2.ClientID %>').prop('checked', false);
                                                            BirthHis_Terms_3_Check();
                                                        }
                                                    }
                                                    function BirthHis_Terms_2_Click() {
                                                        var ctl = $('#<%=BirthHis_Terms_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BirthHis_Terms_1.ClientID %>').prop('checked', false);
                                                            BirthHis_Terms_3_Check();
                                                        }
                                                    }
                                                    $(function () {
                                                        BirthHis_Terms_3_Check();
                                                    });
                                                    function BirthHis_Terms_3_Check() {
                                                        var ctl = $('#<%=BirthHis_Terms_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BirthHis_TermWeek.ClientID %>').removeAttr('disabled');
                                                        } else {
                                                            $('#<%=BirthHis_TermWeek.ClientID %>').val('');
                                                            $('#<%=BirthHis_TermWeek.ClientID %>').attr('disabled', 'disabled');
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="span2">
                                            By how many Weeks
                                        </div>
                                        <div class="span3">
                                            <asp:TextBox ID="BirthHis_TermWeek" runat="server" Enabled="false" class="span3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Delivery :
                                            </div>
                                        </div>
                                        <div class="span9">
                                            <div class="control-label">
                                                <asp:CheckBox ID="BirthHis_Delivery_1" runat="server" Enabled="false" onclick="BirthHis_Delivery_1_Click();"
                                                    CssClass="checkboes" Text=" Normal" />
                                                <asp:CheckBox ID="BirthHis_Delivery_2" runat="server" Enabled="false" onclick="BirthHis_Delivery_2_Click();"
                                                    CssClass="checkboes" Text=" LSCS" />
                                                <script type="text/javascript">
                                                    function BirthHis_Delivery_1_Click() {
                                                        var ctl = $('#<%=BirthHis_Delivery_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BirthHis_Delivery_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function BirthHis_Delivery_2_Click() {
                                                        var ctl = $('#<%=BirthHis_Delivery_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BirthHis_Delivery_1.ClientID %>').prop('checked', false);
                                                        }
                                                    } 
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Length of total labour :
                                            </div>
                                        </div>
                                        <div class="span9">
                                            <div class="control-label">
                                                <asp:TextBox ID="BirthHis_LabourTotal" runat="server" Enabled="false" CssClass="span5"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Difficult labour :
                                            </div>
                                        </div>
                                        <div class="span9">
                                            <div class="control-label">
                                                <asp:CheckBox ID="BirthHis_LabourDiff_1" runat="server" Enabled="false" onclick="BirthHis_LabourDiff_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="BirthHis_LabourDiff_2" runat="server" Enabled="false" onclick="BirthHis_LabourDiff_2_Click();"
                                                    CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function BirthHis_LabourDiff_1_Click() {
                                                        var ctl = $('#<%=BirthHis_LabourDiff_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BirthHis_LabourDiff_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function BirthHis_LabourDiff_2_Click() {
                                                        var ctl = $('#<%=BirthHis_LabourDiff_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BirthHis_LabourDiff_1.ClientID %>').prop('checked', false);
                                                        }
                                                    } 
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                Problems encountered during labour :
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-label">
                                                <asp:TextBox ID="BirthHis_LabourProb" runat="server" Enabled="false" CssClass="span4"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                What kind of aneshthesia if LSCS :
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-label">
                                                <asp:TextBox ID="BirthHis_Aneshthesia" runat="server" Enabled="false" CssClass="span4"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                <h6>
                                                    Others :</h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                CIAB :
                                            </div>
                                        </div>
                                        <div class="span9">
                                            <div class="control-label">
                                                <asp:CheckBox ID="Other_CIAB_1" runat="server" Enabled="false" onclick="Other_CIAB_1_Click();" CssClass="checkboes"
                                                    Text=" Yes" />
                                                <asp:CheckBox ID="Other_CIAB_2" runat="server" Enabled="false" onclick="Other_CIAB_2_Click();" CssClass="checkboes"
                                                    Text=" No" />
                                                <script type="text/javascript">
                                                    function Other_CIAB_1_Click() {
                                                        var ctl = $('#<%=Other_CIAB_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Other_CIAB_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Other_CIAB_2_Click() {
                                                        var ctl = $('#<%=Other_CIAB_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Other_CIAB_1.ClientID %>').prop('checked', false);
                                                        }
                                                    } 
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                Birth Weight :
                                            </div>
                                        </div>
                                        <div class="span3">
                                            <asp:TextBox ID="Other_BirthWeight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                        </div>
                                        <div class="span4">
                                            <div class="control-label">
                                                <asp:CheckBox ID="Other_SGA_AGA_1" runat="server" Enabled="false" onclick="Other_SGA_AGA_1_Click();"
                                                    CssClass="checkboes" Text=" SGA" />
                                                <asp:CheckBox ID="Other_SGA_AGA_2" runat="server" Enabled="false" onclick="Other_SGA_AGA_2_Click();"
                                                    CssClass="checkboes" Text=" AGA" />
                                                <script type="text/javascript">
                                                    function Other_SGA_AGA_1_Click() {
                                                        var ctl = $('#<%=Other_SGA_AGA_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Other_SGA_AGA_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Other_SGA_AGA_2_Click() {
                                                        var ctl = $('#<%=Other_SGA_AGA_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Other_SGA_AGA_1.ClientID %>').prop('checked', false);
                                                        }
                                                    } 
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                APGAR score :
                                            </div>
                                        </div>
                                        <div class="span3">
                                            <asp:TextBox ID="Other_APGAR_Score" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Surgical History">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span2">
                                            <div class="control-label">
                                                NICU :
                                            </div>
                                        </div>
                                        <div class="span2">
                                            <div class="control-label">
                                                <asp:CheckBox ID="NICU_1" runat="server" Enabled="false" onclick="NICU_1_Click();" CssClass="checkboes"
                                                    Text=" Yes" />
                                                <asp:CheckBox ID="NICU_2" runat="server" Enabled="false" onclick="NICU_2_Click();" CssClass="checkboes"
                                                    Text=" No" />
                                                <script type="text/javascript">
                                                    function NICU_1_Click() {
                                                        var ctl = $('#<%=NICU_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=NICU_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function NICU_2_Click() {
                                                        var ctl = $('#<%=NICU_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=NICU_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="">
                                                <div class="control-label span1">
                                                    Duration
                                                </div>
                                                <div class="span4">
                                                    <asp:TextBox ID="NICU_Duration" runat="server" Enabled="false" CssClass="span4"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="">
                                                <div class="control-label span1">
                                                    Reason
                                                </div>
                                                <div class="span4">
                                                    <asp:TextBox ID="NICU_Reason" runat="server" Enabled="false" CssClass="span4"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                Discharged on which day :
                                            </div>
                                        </div>
                                        <div class="span8">
                                            <div class="control-label">
                                                <asp:TextBox ID="DischargedOnWhichDay" runat="server" Enabled="false" CssClass="span5"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                Child taking mother’s feeds :
                                            </div>
                                        </div>
                                        <div class="span8">
                                            <div class="control-label">
                                                <asp:TextBox ID="ChildTakingMotherFeeds" runat="server" Enabled="false" CssClass="span5"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                Any other relevant Medical History :
                                            </div>
                                        </div>
                                        <div class="span8">
                                            <div class="control-label">
                                                <asp:TextBox ID="AnyOtherRelevantMedicalHistory" runat="server" Enabled="false" CssClass="span5"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                Medical Time Line :
                                            </div>
                                        </div>
                                        <div class="span8">
                                            <div class="control-label">
                                                <asp:TextBox ID="MedicalTimeLine" runat="server" Enabled="false" CssClass="span5"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Post Discharge">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                How was the baby at home ?
                                            </div>
                                        </div>
                                        <div class="span8">
                                            <div class="control-label">
                                                <asp:CheckBox ID="HowWasBabyAtHome_1" runat="server" Enabled="false" CssClass="checkboes" Text=" Comfortable" />
                                                <asp:CheckBox ID="HowWasBabyAtHome_2" runat="server" Enabled="false" CssClass="checkboes" Text=" Irritated" />
                                                <asp:CheckBox ID="HowWasBabyAtHome_3" runat="server" Enabled="false" CssClass="checkboes" Text=" Cranky" />
                                                <asp:CheckBox ID="HowWasBabyAtHome_4" runat="server" Enabled="false" CssClass="checkboes" Text=" Playful" />
                                                <asp:CheckBox ID="HowWasBabyAtHome_5" runat="server" Enabled="false" CssClass="checkboes" Text=" Passive" />
                                                <asp:CheckBox ID="HowWasBabyAtHome_6" runat="server" Enabled="false" CssClass="checkboes" Text=" Calm" />
                                                <asp:CheckBox ID="HowWasBabyAtHome_7" runat="server" Enabled="false" CssClass="checkboes" Text=" Well Connected" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                Was he feeding well ?
                                            </div>
                                        </div>
                                        <div class="span8">
                                            <div class="control-label">
                                                <asp:CheckBox ID="WasHeFeedingWell_1" runat="server" Enabled="false" onclick="WasHeFeedingWell_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="WasHeFeedingWell_2" runat="server" Enabled="false" onclick="WasHeFeedingWell_2_Click();"
                                                    CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function WasHeFeedingWell_1_Click() {
                                                        var ctl = $('#<%=WasHeFeedingWell_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=WasHeFeedingWell_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function WasHeFeedingWell_2_Click() {
                                                        var ctl = $('#<%=WasHeFeedingWell_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=WasHeFeedingWell_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                Was he sleeping well ?
                                            </div>
                                        </div>
                                        <div class="span8">
                                            <div class="control-label">
                                                <asp:CheckBox ID="WasHeSleepingWell_1" runat="server" Enabled="false" onclick="WasHeSleepingWell_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="WasHeSleepingWell_2" runat="server" Enabled="false" onclick="WasHeSleepingWell_2_Click();"
                                                    CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function WasHeSleepingWell_1_Click() {
                                                        var ctl = $('#<%=WasHeSleepingWell_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=WasHeSleepingWell_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function WasHeSleepingWell_2_Click() {
                                                        var ctl = $('#<%=WasHeSleepingWell_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=WasHeSleepingWell_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                Any delay? Medical Event? Symptoms?
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="AnyDelay_MedicalEvent_Symptoms" runat="server" Enabled="false" CssClass="span10"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                Who was the first to notice?
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="WhoWasTheFirstNotice" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                What was done for the same (whose consultation, investigation, diagnosis and treatment
                                                plan given?)
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="WhatWasDoneForTheSame" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                Please tell us when your child started to head hold, roll to side, crawl, creep,
                                                sit by himself, stand with and without support and walk by him/herself:
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="ChildStartedToHeadHold" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                Was it on time or delayed??
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="WasItOnTimeOrDelayed" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                Name of others closely involved with child:
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="CloselyInvolvedWithChild" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                Relevent medical time line:
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="ReleventMedicalTimeline" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                How does your child choose to use his/her free time?
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="ChildChooseToUseFreeTime" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                Observations during free play:
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="ObservationsDuringFreePlay" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                Please tell us how do you look after your child’s daily activities Does he/ she
                                                require assistance or can do it independently.
                                            </div>
                                        </div>
                                        <div class="span11">
                                            <table class="table table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                        </th>
                                                        <th>
                                                            Dependent
                                                        </th>
                                                        <th>
                                                            Independent
                                                        </th>
                                                        <th>
                                                            Assisted
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            Brushing
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Brushing_Dependant" runat="server" Enabled="false" onclick="Brushing_Dependant_Click  ();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Brushing_Independant" runat="server" Enabled="false" onclick="Brushing_Independant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Brushing_Assisted" runat="server" Enabled="false" onclick="Brushing_Assisted_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <script type="text/javascript">
                                                            function Brushing_Dependant_Click() {
                                                                var ctl = $('#<%=Brushing_Dependant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Brushing_Independant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Brushing_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Brushing_Independant_Click() {
                                                                var ctl = $('#<%=Brushing_Independant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Brushing_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Brushing_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Brushing_Assisted_Click() {
                                                                var ctl = $('#<%=Brushing_Assisted.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Brushing_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Brushing_Independant.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                        </script>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Toileting
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Toileting_Dependant" runat="server" Enabled="false" onclick="Toileting_Dependant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Toileting_Independant" runat="server" Enabled="false" onclick="Toileting_Independant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Toileting_Assisted" runat="server" Enabled="false" onclick="Toileting_Assisted_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <script type="text/javascript">
                                                            function Toileting_Dependant_Click() {
                                                                var ctl = $('#<%=Toileting_Dependant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Toileting_Independant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Toileting_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Toileting_Independant_Click() {
                                                                var ctl = $('#<%=Toileting_Independant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Toileting_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Toileting_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Toileting_Assisted_Click() {
                                                                var ctl = $('#<%=Toileting_Assisted.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Toileting_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Toileting_Independant.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                        </script>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Bathing
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Bathing_Dependant" runat="server" Enabled="false" onclick="Bathing_Dependant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Bathing_Independant" runat="server" Enabled="false" onclick="Bathing_Independant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Bathing_Assisted" runat="server" Enabled="false" onclick="Bathing_Assisted_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <script type="text/javascript">
                                                            function Bathing_Dependant_Click() {
                                                                var ctl = $('#<%=Bathing_Dependant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Bathing_Independant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Bathing_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Bathing_Independant_Click() {
                                                                var ctl = $('#<%=Bathing_Independant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Bathing_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Bathing_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Bathing_Assisted_Click() {
                                                                var ctl = $('#<%=Bathing_Assisted.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Bathing_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Bathing_Independant.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                        </script>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Dressing
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Dressing_Dependant" runat="server" Enabled="false" onclick="Dressing_Dependant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Dressing_Independant" runat="server" Enabled="false" onclick="Dressing_Independant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Dressing_Assisted" runat="server" Enabled="false" onclick="Dressing_Assisted_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <script type="text/javascript">
                                                            function Dressing_Dependant_Click() {
                                                                var ctl = $('#<%=Dressing_Dependant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Dressing_Independant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Dressing_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Dressing_Independant_Click() {
                                                                var ctl = $('#<%=Dressing_Independant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Dressing_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Dressing_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Dressing_Assisted_Click() {
                                                                var ctl = $('#<%=Dressing_Assisted.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Dressing_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Dressing_Independant.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                        </script>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Feeding
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Feeding_Dependant" runat="server" Enabled="false" onclick="Feeding_Dependant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Feeding_Independant" runat="server" Enabled="false" onclick="Feeding_Independant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Feeding_Assisted" runat="server" Enabled="false" onclick="Feeding_Assisted_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <script type="text/javascript">
                                                            function Feeding_Dependant_Click() {
                                                                var ctl = $('#<%=Feeding_Dependant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Feeding_Independant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Feeding_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Feeding_Independant_Click() {
                                                                var ctl = $('#<%=Feeding_Independant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Feeding_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Feeding_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Feeding_Assisted_Click() {
                                                                var ctl = $('#<%=Feeding_Assisted.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Feeding_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Feeding_Independant.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                        </script>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Ambulation
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Ambulation_Dependant" runat="server" Enabled="false" onclick="Ambulation_Dependant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Ambulation_Independant" runat="server" Enabled="false" onclick="Ambulation_Independant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Ambulation_Assisted" runat="server" Enabled="false" onclick="Ambulation_Assisted_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <script type="text/javascript">
                                                            function Ambulation_Dependant_Click() {
                                                                var ctl = $('#<%=Ambulation_Dependant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Ambulation_Independant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Ambulation_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Ambulation_Independant_Click() {
                                                                var ctl = $('#<%=Ambulation_Independant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Ambulation_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Ambulation_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Ambulation_Assisted_Click() {
                                                                var ctl = $('#<%=Ambulation_Assisted.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Ambulation_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Ambulation_Independant.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                        </script>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Transfer
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Transfer_Dependant" runat="server" Enabled="false" onclick="Transfer_Dependant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Transfer_Independant" runat="server" Enabled="false" onclick="Transfer_Independant_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="Transfer_Assisted" runat="server" Enabled="false" onclick="Transfer_Assisted_Click();"
                                                                CssClass="checkboes" />
                                                        </td>
                                                        <script type="text/javascript">
                                                            function Transfer_Dependant_Click() {
                                                                var ctl = $('#<%=Transfer_Dependant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Transfer_Independant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Transfer_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Transfer_Independant_Click() {
                                                                var ctl = $('#<%=Transfer_Independant.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Transfer_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Transfer_Assisted.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                            function Transfer_Assisted_Click() {
                                                                var ctl = $('#<%=Transfer_Assisted.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=Transfer_Dependant.ClientID %>').prop('checked', false);
                                                                    $('#<%=Transfer_Independant.ClientID %>').prop('checked', false);
                                                                }
                                                            }
                                                        </script>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                Daily Routine :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="DailyRoutine" runat="server" CssClass="span10" Enabled="false" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span11">
                                            <div class="control-label">
                                                Summary :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Summary" runat="server" CssClass="span10" Enabled="false" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div> 
                                    <div class="formRow">
                                        <div class="span3">
                                            <div class="control-label">
                                                Evaluation needed ?
                                            </div>
                                        </div>
                                        <div class="span8">
                                            <div class="control-label">
                                                <asp:CheckBox ID="EvaluationNeeded_1" runat="server" Enabled="false" onclick="EvaluationNeeded_1_Click();"
                                                    CssClass="checkboes" Text=" NDT" />
                                                <asp:CheckBox ID="EvaluationNeeded_2" runat="server" Enabled="false" onclick="EvaluationNeeded_2_Click();"
                                                    CssClass="checkboes" Text=" SI" />
                                                <asp:CheckBox ID="EvaluationNeeded_3" runat="server" Enabled="false" onclick="EvaluationNeeded_3_Click();"
                                                    CssClass="checkboes" Text=" Both" />
                                                <asp:CheckBox ID="EvaluationNeeded_4" runat="server" Enabled="false" onclick="EvaluationNeeded_4_Click();"
                                                    CssClass="checkboes" Text=" Ortho" />
                                                <asp:CheckBox ID="EvaluationNeeded_5" runat="server" Enabled="false" onclick="EvaluationNeeded_5_Click();"
                                                    CssClass="checkboes" Text=" EIP" />
                                                <asp:CheckBox ID="EvaluationNeeded_6" runat="server" Enabled="false" onclick="EvaluationNeeded_6_Click();"
                                                    CssClass="checkboes" Text=" General" />
                                                <script type="text/javascript">
                                                    function EvaluationNeeded_1_Click() {
                                                        var ctl = $('#<%=EvaluationNeeded_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=EvaluationNeeded_2.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_3.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_4.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_5.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_6.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function EvaluationNeeded_2_Click() {
                                                        var ctl = $('#<%=EvaluationNeeded_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=EvaluationNeeded_1.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_3.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_4.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_5.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_6.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function EvaluationNeeded_3_Click() {
                                                        var ctl = $('#<%=EvaluationNeeded_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=EvaluationNeeded_1.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_2.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_4.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_5.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_6.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function EvaluationNeeded_4_Click() {
                                                        var ctl = $('#<%=EvaluationNeeded_4.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=EvaluationNeeded_1.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_2.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_3.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_5.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_6.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function EvaluationNeeded_5_Click() {
                                                        var ctl = $('#<%=EvaluationNeeded_5.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=EvaluationNeeded_1.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_2.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_3.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_4.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_6.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function EvaluationNeeded_6_Click() {
                                                        var ctl = $('#<%=EvaluationNeeded_6.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=EvaluationNeeded_1.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_2.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_3.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_4.ClientID %>').prop('checked', false);
                                                            $('#<%=EvaluationNeeded_5.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Speciality Contacts">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span11">
                                            <table class="table table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                        </th>
                                                        <th>
                                                            Name of Agency
                                                        </th>
                                                        <th>
                                                            Specialist Date
                                                        </th>
                                                        <th>
                                                            Address
                                                        </th>
                                                        <th>
                                                            Phone
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            Cardiologist
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Cardiologist_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Cardiologist_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Cardiologist_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Cardiologist_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Orthopedist
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Orthopedist_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Orthopedist_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Orthopedist_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Orthopedist_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Psychologist
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Psychologist_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Psychologist_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Psychologist_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Psychologist_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Psychiatrist
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Psychiatrist_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Psychiatrist_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Psychiatrist_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Psychiatrist_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Opthalmologist/ Optometrist
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Opthalmologist_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Opthalmologist_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Opthalmologist_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Opthalmologist_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Speech
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Speech_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Speech_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Speech_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Speech_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Pathologist
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Pathologist_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Pathologist_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Pathologist_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Pathologist_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Occupational Therapist
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Occupational_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Occupational_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Occupational_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Occupational_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Physical Therapist
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Physical_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Physical_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Physical_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Physical_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Audiologist
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Audiologist_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Audiologist_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Audiologist_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Audiologist_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            ENT
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ENT_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ENT_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ENT_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ENT_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Chiropractor
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Chiropractor_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Chiropractor_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Chiropractor_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Chiropractor_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Other
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Other_Name" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Other_Date" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Other_Addr" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Other_Phone" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tb_Report21" runat="server" HeaderText="Doctor">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span11">
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
                                        <div class="span11">
                                            <div class="control-label">
                                                2. Occupational Therapist :
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
                                        <div class="span11">
                                            <div class="control-label">
                                                3. Name of Director :
                                            </div>
                                            <div class="control-group">
                                                <asp:DropDownList ID="Doctor_EnterReport" runat="server" Enabled="false"  CssClass="chzn-select span6">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
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
                    <div class="clearfix">
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="clearfix">
            </div>
        </div>
    </div>
</asp:Content>
