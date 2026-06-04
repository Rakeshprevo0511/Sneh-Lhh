<%@ Page Title="" MasterPageFile="~/Member/Site.master" Language="C#" AutoEventWireup="true" CodeBehind="PreConsultRpt.aspx.cs" Inherits="snehrehab.SessionRpt.PreConsultRpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .hide {
        display: none !important;
    }
</style>
    <style type="text/css">
        .Morphology-OuterTopTable {
        }

            .Morphology-OuterTopTable tr td {
                padding: 5px;
                border: 1px solid #ccc;
                text-align: center;
            }

        .Morphology-Upper-Limb {
        }

            .Morphology-Upper-Limb tr td {
                padding: 5px;
                border: 1px solid #CCC;
            }

        .Morphology-Lower-Limb {
        }

            .Morphology-Lower-Limb tr td {
                padding: 5px;
                border: 1px solid #CCC;
            }

        .ndt-default-table {
        }

            .ndt-default-table tr td {
                border: 1px solid #ccc;
                padding: 10px;
            }

        span.char-limit-msg {
            font-style: italic;
            color: red;
            font-size: 11px;
        }

        .checkboes {
            float: left;
            margin-right: 10px;
        }

        .lable_textarea {
            position: initial;
            margin: 21px auto;
        }

        .lable_text {
            position: absolute;
            margin: 50px auto;
        }

        .save-status {
            margin-right: 15px
        }

        .buttonClass
        {
           background-color:springgreen;
        }
        .row-actions-vertical {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 4px;
}

.row-actions-vertical .action-btn {
    display: block;
    width: 28px;   /* optional: uniform width */
    text-align: center;
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
    <script type="text/javascript" src="js/36d/jquery-1.4.2.min.js"></script>
    <%--</script>--%>
    <script type="text/javascript">  
        $(document).ready(function () {
            $("form").bind("keypress", function (e) {
                if (e.keyCode == 13) {
                    return false;
                }
            });
        });
    </script>
  <script type="text/javascript">
      function remove_row(btn) {

          var row = $(btn).closest(".cloneThisRow");
          var visibleRows = $(".cloneThisRow:not(.hide)");

          // ❌ Do not allow delete if only one row left
          if (visibleRows.length <= 1) {
              return;
          }

          // Get DB ID
          var preConsultID = row.find("input[id*='txtPreConsultID']").val();

          // ✅ Add ONLY THIS row's ID
          if (preConsultID && preConsultID !== "0") {

              var deleted = $('#<%= hdnDeletedTimelineIds.ClientID %>').val();

              // prevent duplicates
              if (deleted.split(',').indexOf(preConsultID) === -1) {
                  deleted = deleted ? deleted + "," + preConsultID : preConsultID;
                  $('#<%= hdnDeletedTimelineIds.ClientID %>').val(deleted);
              }
          }

          // ✅ Remove only the clicked row
          row.remove();
      }
      function move_up(btn) {
          var row = $(btn).closest(".cloneThisRow");
          var prev = row.prevAll(".cloneThisRow:not(.hide)").first();

          if (prev.length) {
              prev.before(row);   // REAL DOM reorder
          }
      }

      function move_down(btn) {
          var row = $(btn).closest(".cloneThisRow");
          var next = row.nextAll(".cloneThisRow:not(.hide)").first();

          if (next.length) {
              next.after(row);    // REAL DOM reorder
          }
      }

      $('#option_box_single_choice').show();

      var total_to_view = parseInt($('#<%= txtVisibleOption.ClientID %>').val());
      if (isNaN(total_to_view)) { total_to_view = 0; }
      if (total_to_view <= 1) { total_to_view = 1; }

      var ctls = $('#option_box_single_choice').find('.cloneThisRow');

      for (var i = 1; i <= ctls.length; i++) {
          if (i <= total_to_view) {
              $(ctls[i - 1]).removeClass('hide');
          } else {
              $(ctls[i - 1]).addClass('hide');
          }
      }

      AddRemoveButton($(ctls[0]));

      // ➕ ADD ROW
      function show_next_option(ctl) {
          var cloneThisRowAdded = $(ctl)
              .parents('.cloneContainer')
              .children('.cloneThisRow.hide:first');

          if (cloneThisRowAdded.length === 0) return;

          $(cloneThisRowAdded).removeClass('hide');
          $(cloneThisRowAdded).find('input[type="text"], textarea').val('');
          $(cloneThisRowAdded).find('input[type="hidden"]').val('0');

          AddRemoveButton(ctl);
      }

      // ➖ ADD / REMOVE BUTTON HANDLER
      function AddRemoveButton(ctl) {

          var ctls = $(ctl)
              .parents('.cloneContainer')
              .find('.cloneThisRow:not(.hide)');

          $('#<%= txtVisibleOption.ClientID %>').val(ctls.length);

          ctls.find('.rbutton').html('');

          if (ctls.length > 2) {
              $(ctls[ctls.length - 1])
                  .find('.rbutton')
                  .html(
                      '<a href="javascript:;" class="btn btn-xs btn-danger" ' +
                      'onclick="remove_this_option(this)">' +
                      '<i class="fa fa-minus"></i></a>'
                  );
          }
      }

      // ❌ REMOVE (HIDE) ROW
      function remove_this_option(ctl) {

          var cloneThisRowRemoved = $(ctl).closest('.cloneThisRow');

          cloneThisRowRemoved.addClass('hide');

          // clear all values so nothing is saved
          cloneThisRowRemoved
              .find('input[type="text"], textarea')
              .val('');

          cloneThisRowRemoved
              .find('input[type="hidden"]')
              .val('0');

          AddRemoveButton(ctl);
      }

  </script>

    <script type="text/javascript">

        function update() {

            $('.save-status').text("Saving...");
            DataJson =
                '{favLang:"' + $('#<%= txtComfortableLanguage.ClientID%>').val() +
                '", txtCorrectAge: "' + $('#<%= txtCorrectAge.ClientID%>').val() +
            '", txtAge: "' + $('#<%= txtAge.ClientID%>').val() +
            '", txtMotherName: "' + $('#<%= txtMotherName.ClientID%>').val() +
            '", txtMotherAge: "' + $('#<%= txtMotherAge.ClientID%>').val() +
            '", txtMotherQualification: "' + $('#<%= txtMotherQualification.ClientID%>').val() +
            '", txtMotherOccupation: "' + $('#<%= txtMotherOccupation.ClientID%>').val() +
            '", txtMotherWorkingHour: "' + $('#<%= txtMotherWorkingHour.ClientID%>').val() +
            '", txtFatherName: "' + $('#<%= txtFatherName.ClientID%>').val() +
            '", txtFatherAge: "' + $('#<%= txtFatherAge.ClientID%>').val() +
            '", txtFatherOccupation: "' + $('#<%= txtFatherOccupation.ClientID%>').val() +
            '", txtFatherQualification: "' + $('#<%= txtFatherQualification.ClientID%>').val() +
            '", txtFatherWorkingHour: "' + $('#<%= txtFatherWorkingHour.ClientID%>').val() +
            '", txtAddress: "' + $('#<%= txtAddress.ClientID%>').val() +
            '", txtContactDetails: "' + $('#<%= txtContactDetails.ClientID%>').val() +
            '", txtEmailID: "' + $('#<%= txtEmailID.ClientID%>').val() +
            '", txtReferredBy: "' + $('#<%= txtReferredBy.ClientID%>').val() +
            '", txtTherapistDuringPC: "' + $('#<%= txtTherapistDuringPC.ClientID%>').val() +
            '", txtDiagnosis: "' + $('#<%= txtDiagnosis.ClientID%>').val() +
            '", txtCommentsPI: "' + $('#<%= txtCommentsPI.ClientID%>').val() +
            '", txtChiefConcernsHome: "' + $('#<%= txtChiefConcernsHome.ClientID%>').val() +
            '", txtChiefConcernsSchool: "' + $('#<%= txtChiefConcernsSchool.ClientID%>').val() +
            '", txtChiefConcernsSocialGath: "' + $('#<%= txtChiefConcernsSocialGath.ClientID%>').val() +
            '", txtCommentsCC: "' + $('#<%= txtCommentsCC.ClientID%>').val() +
            '", txtYearsMarriage: "' + $('#<%= txtYearsMarriage.ClientID%>').val() +
            '", txtCommentsFH: "' + $('#<%= txtCommentsFH.ClientID%>').val() +
            '", txtfrequency: "' + $('#<%= txtfrequency.ClientID%>').val() +
            '", txtMotherScreenTime: "' + $('#<%= txtMotherScreenTime.ClientID%>').val() +
            '", txtScreenTimeChild: "' + $('#<%= txtScreenTimeChild.ClientID%>').val() +
            '", txtCommentsFR: "' + $('#<%= txtCommentsFR.ClientID%>').val() +
            '", txtPrenatalCondition: "' + $('#<%= txtPrenatalCondition.ClientID%>').val() +
            '", txtDescribeStressors: "' + $('#<%= txtDescribeStressors.ClientID%>').val() +
            '", txtWGDP: "' + $('#<%= txtWGDP.ClientID%>').val() +
            '", txtFoetalMovement: "' + $('#<%= txtFoetalMovement.ClientID%>').val() +
            '", txtCommentsMH: "' + $('#<%= txtCommentsMH.ClientID%>').val() +
            '", txtDurationLabour: "' + $('#<%= txtDurationLabour.ClientID%>').val() +
            '", txtConditionPostBirth: "' + $('#<%= txtConditionPostBirth.ClientID%>').val() +
            '", txtBirthWeight: "' + $('#<%= txtBirthWeight.ClientID%>').val() +
            '", txtDurationNICUstay: "' + $('#<%= txtDurationNICUstay.ClientID%>').val() +
            '", txtNICUHistory: "' + $('#<%= txtNICUHistory.ClientID%>').val() +
            '", txtReasonNICUstay: "' + $('#<%= txtReasonNICUstay.ClientID%>').val() +
            '", txtAPGARscore: "' + $('#<%= txtAPGARscore.ClientID%>').val() +
            '", txtBabyFed: "' + $('#<%= txtBabyFed.ClientID%>').val() +
            '", txtMentionProblem: "' + $('#<%= txtMentionProblem.ClientID%>').val() +
            '", txtwaswtcbf: "' + $('#<%= txtwaswtcbf.ClientID%>').val() +
            '", txtOthrtMedicalIssues: "' + $('#<%= txtOthrtMedicalIssues.ClientID%>').val() +
            '", txtCommentsPPH: "' + $('#<%= txtCommentsPPH.ClientID%>').val() +
            '", txtGrossMotor: "' + $('#<%= txtGrossMotor.ClientID%>').val() +
            '", txtFineMotor: "' + $('#<%= txtFineMotor.ClientID%>').val() +
            '", txtPersonalandSocial: "' + $('#<%= txtPersonalandSocial.ClientID%>').val() +
            '", txtCommunication: "' + $('#<%= txtCommunication.ClientID%>').val() +
            '", txtCommentsDM: "' + $('#<%= txtCommentsDM.ClientID%>').val() +
            '", txtSleepduration: "' + $('#<%= txtSleepduration.ClientID%>').val() +
            '", txtCosleepingwith: "' + $('#<%= txtCosleepingwith.ClientID%>').val() +
            '", txtAnySleepAdjunctsused: "' + $('#<%= txtAnySleepAdjunctsused.ClientID%>').val() +
            '", txtNapduration: "' + $('#<%= txtNapduration.ClientID%>').val() +
            '", txtCommentsS: "' + $('#<%= txtCommentsS.ClientID%>').val() +
            '", txtTypeoffoodhad: "' + $('#<%= txtTypeoffoodhad.ClientID%>').val() +
            '", txtFoodconsistency: "' + $('#<%= txtFoodconsistency.ClientID%>').val() +
            '", txtFoodtemperature: "' + $('#<%= txtFoodtemperature.ClientID%>').val() +
            '", txtFoodtaste: "' + $('#<%= txtFoodtaste.ClientID%>').val() +
            '", txtCommentsFeHa: "' + $('#<%= txtCommentsFeHa.ClientID%>').val() +
            '", txtChildLikes: "' + $('#<%= txtChildLikes.ClientID%>').val() +
            '", txtCommentsITCH: "' + $('#<%= txtCommentsITCH.ClientID%>').val() +
            '", txtInteractionwithpeers: "' + $('#<%= txtInteractionwithpeers.ClientID%>').val() +
            '", txtPreferenceoftoys: "' + $('#<%= txtPreferenceoftoys.ClientID%>').val() +
            '", txtCommentsPB: "' + $('#<%= txtCommentsPB.ClientID%>').val() +
            '", txtCommentsBrushing: "' + $('#<%= txtCommentsBrushing.ClientID%>').val() +
            '", txtCommentsBathing: "' + $('#<%= txtCommentsBathing.ClientID%>').val() +
            '", txtCommentsToileting: "' + $('#<%= txtCommentsToileting.ClientID%>').val() +
            '", txtCommentsDressing: "' + $('#<%= txtCommentsDressing.ClientID%>').val() +
            '", txtCommentsEating: "' + $('#<%= txtCommentsEating.ClientID%>').val() +
            '", txtCommentsAmbulation: "' + $('#<%= txtCommentsAmbulation.ClientID%>').val() +
            '", txtCommentsTransfers: "' + $('#<%= txtCommentsTransfers.ClientID%>').val() +
            '", txtAddComments: "' + $('#<%= txtAddComments.ClientID%>').val() +
            '", txtNoOfSiblings: "' + $('#<%= txtNoOfSiblings.ClientID%>').val() +
            '", txtRHASiblings: "' + $('#<%= txtRHASiblings.ClientID%>').val() +
            '", txtAddEvalRec: "' + $('#<%= txtAddEvalRec.ClientID%>').val() +
            '", txtOnlineOffline: "' + $('#<%= txtOnlineOffline.ClientID%>').val() +
            '", txtWhichGrade: "' + $('#<%= txtWhichGrade.ClientID%>').val() + '" }',

                $.ajax({
                    type: "POST",
                    url: "PreConsultRpt.aspx/SaveData",
                    data: DataJson,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('.save-status').text(response.d);
                    },
                    failure: function (response) {
                        $('.save-status').text(response.d);
                    }
                })

        }

        $(function () {
            $('.savedata').keyup(function (e) {
                //update();
            });
        });

    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1" ID="Content2">
        <div id="MsgPlaceHolder"></div>
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Pre-Consultation Report :
            </div>
            <div class="pull-right">
                <a class="save-status"></a>
                <a href="/Reports/PreConsultation.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">Patient Name :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtPatient" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">Session :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtSession" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">Mark as Report Final :</label>
                    <div class="control-group">
                        <asp:CheckBox ID="txtFinal" runat="server" />
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">Mark as Report Given :</label>
                    <div class="control-group">
                        <asp:CheckBox ID="txtGiven" runat="server" />
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
           <div class="formRow">
    <div class="span6">
        <label class="control-label">Given Date :</label>
        <div class="control-group">
            <asp:TextBox ID="txtGivenDate" runat="server"
                CssClass="span2 my-datepicker" AutoPostBack="true"></asp:TextBox>
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
                    <label class="control-label">&nbsp;</label>
                    <div class="control-group">
                              <button type="button" id="btnFinalSubmit" class="btn btn-danger">
    Submit
</button>
                        &nbsp;
                        <%= _printUrl %>
                       <%-- <%= _resercherUrl %>--%>
                        <a href='<%= _cancelUrl %>' class="btn btn-default">Cancel</a>
                        <asp:HiddenField ID="txtPrint" runat="server" />
                       <%-- <asp:HiddenField ID="hidimpprint" runat="server" />--%>
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
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="clearfix"></div>
            <div class="formRow">
                <div class="span12">
                    <hr />
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="formRow">
                <div class="span12">
                    <asp:HiddenField ID="hfdTabs" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdCallFrom" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdCurTab" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdPrevTab" runat="server" ClientIDMode="Static" />

                    <ajaxToolkit:TabContainer ID="tb_Contents" runat="server" OnClientActiveTabChanged="clientActiveTabChanged"  ClientIDMode="Static">
                        <ajaxToolkit:TabPanel ID="TabPanel13" runat="server" HeaderText="Patient Information">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Date of pre-Consultation :
                                            </div>
                                            <asp:TextBox ID="txtDatepreConsult" runat="server" CssClass="span2 my-datepicker" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Language you're comfortable in :
                                            </div>
                                            <asp:TextBox ID="txtComfortableLanguage" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Date of birth :
                                            </div>
                                            <asp:TextBox ID="txtDateBirth" runat="server" CssClass="span2 my-datepicker" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Expected date of delivery :
                                            </div>
                                            <asp:TextBox ID="txtDateofDelivery" runat="server" CssClass="span2 my-datepicker" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Corrected Age - if relevant :
                                            </div>
                                            <asp:TextBox ID="txtCorrectAge" runat="server" CssClass="span2 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Age :
                                            </div>
                                            <asp:TextBox ID="txtAge" runat="server" CssClass="span2 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Gender :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckFemale" runat="server" CssClass="checkboes" onclick="Check_Female_Click();" Text="Female" />
                                                    <asp:CheckBox ID="CheckMale" runat="server" CssClass="checkboes" onclick="Check_Male_Click();" Text="Male" />
                                                    <asp:CheckBox ID="CheckOther" runat="server" CssClass="checkboes" onclick="Check_Other_Click();" Text="Other" />
                                                    <script type="text/javascript">
                                                        function Check_Female_Click() {
                                                            var ctl = $('#<%=CheckFemale.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckMale.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckOther.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Male_Click() {
                                                            var ctl = $('#<%=CheckMale.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckFemale.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckOther.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Other_Click() {
                                                            var ctl = $('#<%=CheckOther.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                5
                                                                $('#<%=CheckFemale.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckMale.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Does Your child attend School ?
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="YesAttend" runat="server" CssClass="checkboes" onclick="YesAttends();" Text="Yes" />
                                                    <asp:CheckBox ID="Noattend" runat="server" CssClass="checkboes" onclick="NoAttends();" Text="No" />
                                                    <script type="text/javascript">
                                                        function YesAttends() {
                                                            var ctl = $('#<%=YesAttend.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Noattend.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function NoAttends() {
                                                            var ctl = $('#<%=Noattend.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=YesAttend.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Which school does your child study in ? Mention online/offline :
                                            </div>
                                            <asp:TextBox ID="txtOnlineOffline" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Which grade ?
                                            </div>
                                            <asp:TextBox ID="txtWhichGrade" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's Name :
                                            </div>
                                            <asp:TextBox ID="txtMotherName" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's current age  
                                            </div>
                                            <asp:TextBox ID="txtMotherAge" runat="server" CssClass="span2 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--<div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's age during conception
                                            </div>
                                            <asp:TextBox ID="txtMotherAgeDC" runat="server" CssClass="span2"></asp:TextBox>
                                        </div>
                                    </div>--%>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's Qualification :
                                            </div>
                                            <asp:TextBox ID="txtMotherQualification" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's Occupation :
                                            </div>
                                            <asp:TextBox ID="txtMotherOccupation" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's Working Hours :
                                            </div>
                                            <asp:TextBox ID="txtMotherWorkingHour" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's Name :
                                            </div>
                                            <asp:TextBox ID="txtFatherName" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's current age :
                                            </div>
                                            <asp:TextBox ID="txtFatherAge" runat="server" CssClass="span2 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--<div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's age during conception
                                            </div>
                                            <asp:TextBox ID="txtFatherAgeDC" runat="server" CssClass="span2"></asp:TextBox>
                                        </div>
                                    </div>--%>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's Occupation :
                                            </div>
                                            <asp:TextBox ID="txtFatherOccupation" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's Qualification :
                                            </div>
                                            <asp:TextBox ID="txtFatherQualification" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's Working Hours :
                                            </div>
                                            <asp:TextBox ID="txtFatherWorkingHour" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Address :
                                            </div>
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Contact details - Mother and Father :
                                            </div>
                                            <asp:TextBox ID="txtContactDetails" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Parent’s Email id’s :
                                            </div>
                                            <asp:TextBox ID="txtEmailID" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Referred by :
                                            </div>
                                            <asp:TextBox ID="txtReferredBy" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Therapist during pre consultation :
                                            </div>
                                            <asp:TextBox ID="txtTherapistDuringPC" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Diagnosis if any :
                                            </div>
                                            <asp:TextBox ID="txtDiagnosis" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsPI" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Chief Concerns">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Chief concerns at Home :
                                            </div>
                                            <asp:TextBox ID="txtChiefConcernsHome" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Chief concerns at School :
                                            </div>
                                            <asp:TextBox ID="txtChiefConcernsSchool" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Chief concerns at social gatherings :
                                            </div>
                                            <asp:TextBox ID="txtChiefConcernsSocialGath" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsCC" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel0" runat="server" HeaderText="Timeline">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnDeletedTimelineIds" runat="server" />
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="col-md-12">
                                            <ul style="display: flex; list-style-type: none; justify-content: space-evenly;">
                                                <li style="margin-left: -125px;">
                                                    <lable>Date/Month</lable></li>
                                                <li>
                                                    <lable>RelevantHistory</lable></li>
                                                <li>
                                                    <lable>
                                                        Hospital<br />
                                                        DoctorsVisited</lable></li>
                                                <li>
                                                    <lable>
                                                        Doctors<br />
                                                        Recommendation</lable></li>
                                                <li>
                                                    <lable>
                                                        Investigations<br />
                                                        RecordsResults</lable></li>
                                            </ul>
                                        </div>
                                        <div class="span5">
                                            <div class="control-label">
                                                <asp:HiddenField ID="txtVisibleOption" runat="server" Value="2" />
                                                <div id="option_box_single_choice">
                                                    <div class="form-group row col-sm-12">
                                                        <div class="col-md-10">
                                                            <div class="cloneContainer">
                                                                <asp:Repeater ID="txtSignleChoice" runat="server">
                                                                    <ItemTemplate>
                                                                        <div class='row cloneThisRow <%# cloneClass(Container.ItemIndex, Eval("Option").ToString(), Eval("Option1").ToString(), Eval("Option2").ToString(), Eval("Option3").ToString(), Eval("Option4").ToString(), Eval("Option5").ToString()) %>'>
                                                                            <div class="col-sm-2">
                                                                                <%--<label class="control-label"></label>--%>
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                                <ul class="d-flex" style="display: flex; list-style-type: none;">

                                                                                    <asp:HiddenField ID="txtPreConsultID" runat="server" Value='<%#Eval("Option") %>' />
                                                                                    <%--  <asp:HiddenField ID="txtPreConsultIDtime" runat="server" Value='<%#Eval("Option") %>' />--%>
                                                                                    <li class="mr_5"><%--<input type="text" id="txtDateMonth"/>--%><asp:TextBox ID="txtDateMonth" runat="server" Text='<%#Eval("Option1") %>' TextMode="MultiLine" Style="width: 206px; height: 132px;"></asp:TextBox></li>
                                                                                    <li class="mr_5"><%--<input type="text" id="txtRelevantHistory"/>--%><asp:TextBox ID="txtRelevantHistory" runat="server" Text='<%#Eval("Option2") %>' TextMode="MultiLine" Style="width: 206px; height: 132px;"></asp:TextBox></li>
                                                                                    <li class="mr_5"><%--<input type="text" id="txtHospitalDoctorsVisited"/>--%><asp:TextBox ID="txtHospitalDoctorsVisited" runat="server" Text='<%#Eval("Option3") %>' TextMode="MultiLine" Style="width: 206px; height: 132px;"></asp:TextBox></li>
                                                                                    <li class="mr_5"><%--<input type="text" id="txtDoctorsRecommendations"/>--%><asp:TextBox ID="txtDoctorsRecommendations" runat="server" Text='<%#Eval("Option4") %>' TextMode="MultiLine" Style="width: 206px; height: 132px;"></asp:TextBox></li>
                                                                                    <li class="mr_5"><%--<input type="text" id="txtInvestigationsRecordsResults"/>--%><asp:TextBox ID="txtInvestigationsRecordsResults" runat="server" Text='<%#Eval("Option5") %>' TextMode="MultiLine" Style="width: 206px; height: 132px;"></asp:TextBox></li>
                                                                                    <div class="col-md-3 padding-5">
                                                                                        <%# cloneButtonLeft_sm(Container.ItemIndex)%>
                                                                                    </div>
                                                                                </ul>
                                                                            </div>
                                                                            <div class="col-sm-12">
                                                                                <div class="row">
                                                                                    <div class="col-sm-9">
                                                                                        <%--<asp:TextBox ID="DateMonth" runat="server"  Text='<%#Eval("Option") %>'></asp:TextBox>--%>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>




                                                <%--<table id="myTable">
                                                    <th>Date/Month</th>
                                                    <th>Relevant History</th>
                                                    <th>Hospital/Doctors visited</th>
                                                    <th>Doctors Recommendations</th>
                                                    <th>Investigations/Records results</th>
                                                    <tr>
                                                        <td><input type="text" id="txtDateMonth" /></td>
                                                        <td><input type="text" id="txtRelevantHistory" /></td>
                                                        <td><input type="text" id="txtHospitalDoctorsVisited" /></td>
                                                        <td><input type="text" id="txtDoctorsRecommendations" /></td>
                                                        <td><input type="text" id="txtInvestigationsRecordsResults" /></td>
                                                        <td><input type="button" id="btnAdd" class="<button-ad></button-ad>d" onClick="insertRow()" value="Add"/></td>
                                                    </tr>
                                                 </table>
                                                <script type="text/javascript">
                                                    var index = 1;
                                                    function insertRow() {
                                                        var table = document.getElementById("myTable");
                                                        var row = table.insertRow(table.rows.length);
                                                        var cell1 = row.insertCell(0);
                                                        var t1 = document.createElement("input");
                                                        t1.id = "txtDateMonth" + index;
                                                        cell1.appendChild(t1);
                                                        var cell2 = row.insertCell(1);
                                                        var t2 = document.createElement("input");
                                                        t2.id = "txtRelevantHistory" + index;
                                                        cell2.appendChild(t2);
                                                        var cell3 = row.insertCell(2);
                                                        var t3 = document.createElement("input");
                                                        t3.id = "txtHospitalDoctorsVisited" + index;
                                                        cell3.appendChild(t3);
                                                        var cell4 = row.insertCell(3);
                                                        var t4 = document.createElement("input");
                                                        t4.id = "txtDoctorsRecommendations" + index;
                                                        cell4.appendChild(t4);
                                                        var cell5 = row.insertCell(4);
                                                        var t5 = document.createElement("input");
                                                        t5.id = "txtInvestigationsRecordsResults" + index;
                                                        cell5.appendChild(t5);
                                                        index++;

                                                    }
                                                </script>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Family History and Relations">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Consanguinity :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckConsan" runat="server" CssClass="checkboes" onclick="Consanguineous_Click();" Text="Consanguineous Marriage" />
                                                    <asp:CheckBox ID="CheckNonConsan" runat="server" CssClass="checkboes" onclick="NonConsanguineous_Click();" Text="Non- Consanguineous" />
                                                    <%--<script type="text/javascript">
                                                        function Consanguineous_Click() {
                                                            var ctl = $('#<%=CheckConsan.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNonConsan.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function NonConsanguineous_Click() {
                                                            var ctl = $('#<%=CheckNonConsan.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckConsan.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                If Consanguinous - Degree of Consanguinity :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="Check1Deg" runat="server" CssClass="checkboes" onclick="Check1Deg_Click();" Text="1st deg" />
                                                    <asp:CheckBox ID="Check2Deg" runat="server" CssClass="checkboes" onclick="Check2Deg_Click();" Text="2nd  deg" />
                                                    <asp:CheckBox ID="Check3Deg" runat="server" CssClass="checkboes" onclick="Check3Deg_Click();" Text="3rd  deg" />
                                                    <%--<script type="text/javascript">
                                                        function Check1Deg_Click() {
                                                            var ctl = $('#<%=Check1Deg.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check2Deg.ClientID %>').prop('checked', false);
                                                                $('#<%=Check3Deg.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check2Deg_Click() {
                                                            var ctl = $('#<%=Check2Deg.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check1Deg.ClientID %>').prop('checked', false);
                                                                $('#<%=Check3Deg.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check3Deg_Click() {
                                                            var ctl = $('#<%=Check3Deg.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check1Deg.ClientID %>').prop('checked', false);
                                                                $('#<%=Check2Deg.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span5">
                                            <div class="control-label">
                                                Years of marriage :
                                            </div>
                                            <asp:TextBox ID="txtYearsMarriage" runat="server" CssClass="span2 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Family structure :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckNuclear" runat="server" CssClass="checkboes" onclick="Nuclear_Click();" Text="Nuclear" />
                                                    <asp:CheckBox ID="CheckJoint" runat="server" CssClass="checkboes" onclick="Joint_Click();" Text="Joint" />
                                                    <%--<script type="text/javascript">
                                                        function Nuclear_Click() {
                                                            var ctl = $('#<%=CheckNuclear.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckJoint.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Joint_Click() {
                                                            var ctl = $('#<%=CheckJoint.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNuclear.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Conception :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckNatural" runat="server" CssClass="checkboes" onclick="Natural_Click();" Text="Natural" />
                                                    <asp:CheckBox ID="CheckIUI" runat="server" CssClass="checkboes" onclick="IUI_Click();" Text="IUI" />
                                                    <asp:CheckBox ID="CheckIVF" runat="server" CssClass="checkboes" onclick="IVF_Click();" Text="IVF" />
                                                    <asp:CheckBox ID="CheckISCI" runat="server" CssClass="checkboes" onclick="ISCI_Click();" Text="ISCI" />
                                                    <asp:CheckBox ID="CheckOI" runat="server" CssClass="checkboes" onclick="OI_Click();" Text="OI" />
                                                    <%--<script type="text/javascript">
                                                        function Natural_Click() {
                                                            var ctl = $('#<%=CheckNatural.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckIUI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIVF.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckISCI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckOI.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function IUI_Click() {
                                                            var ctl = $('#<%=CheckIUI.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNatural.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIVF.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckISCI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckOI.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function IVF_Click() {
                                                            var ctl = $('#<%= CheckIVF.ClientID%>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNatural.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIUI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckISCI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckOI.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function ISCI_Click() {
                                                            var ctl = $('#<%= CheckISCI.ClientID%>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNatural.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIUI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIVF.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckOI.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function OI_Click() {
                                                            var ctl = $('#<%= CheckOI.ClientID%>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNatural.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIUI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIVF.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckISCI.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Planning of Conception :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckPlanned" runat="server" CssClass="checkboes" onclick="Planned_Click();" Text="Planned" />
                                                    <asp:CheckBox ID="CheckUnplanned" runat="server" CssClass="checkboes" onclick="Unplanned_Click();" Text="Unplanned" />
                                                    <%--<script type="text/javascript">
                                                        function Planned_Click() {
                                                            var ctl = $('#<%=CheckPlanned.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckUnplanned.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Unplanned_Click() {
                                                            var ctl = $('#<%=CheckUnplanned.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPlanned.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Siblings History :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    Any Siblings ?
                                                </div>
                                                <asp:CheckBox ID="AnySiblingsYes" runat="server" CssClass="checkboes" Text="Yes" />
                                                <asp:CheckBox ID="AnySiblingsNo" runat="server" CssClass="checkboes" Text="No" />
                                            </div>
                                        </div>
                                        <div class="span12">
                                            <div class="span5">
                                                <div class="control-label">
                                                    No of Siblings
                                                </div>
                                                <asp:TextBox ID="txtNoOfSiblings" runat="server" CssClass="span2 savedata"></asp:TextBox>
                                            </div>

                                        </div>
                                        <div class="span12">
                                            <div class="span5">
                                                <div class="control-label">
                                                    Relevant History about siblings
                                                </div>
                                                <asp:TextBox ID="txtRHASiblings" runat="server" CssClass="span2 savedata"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsFH" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>

                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Inter parental relationship :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckPoor" runat="server" CssClass="checkboes" onclick="CheckPoor_Click();" Text="Poor" />
                                                    <asp:CheckBox ID="CheckFair" runat="server" CssClass="checkboes" onclick="CheckFair_Click();" Text="Fair" />
                                                    <asp:CheckBox ID="CheckGood" runat="server" CssClass="checkboes" onclick="CheckGood_Click();" Text="Good" />
                                                    <%--<script type="text/javascript">
                                                        function CheckPoor_Click() {
                                                            var ctl = $('#<%=CheckPoor.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckFair.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGood.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckFair_Click() {
                                                            var ctl = $('#<%=CheckFair.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPoor.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGood.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckGood_Click() {
                                                            var ctl = $('#<%=CheckGood.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPoor.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckFair.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Parent child relationship :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckPoorr" runat="server" CssClass="checkboes" onclick="CheckPoorr_Click();" Text="Poor" />
                                                    <asp:CheckBox ID="CheckFairr" runat="server" CssClass="checkboes" onclick="CheckFairr_Click();" Text="Fair" />
                                                    <asp:CheckBox ID="CheckGoodd" runat="server" CssClass="checkboes" onclick="CheckGoodd_Click();" Text="Good" />
                                                    <%--<script type="text/javascript">
                                                        function CheckPoorr_Click() {
                                                            var ctl = $('#<%=CheckPoorr.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckFairr.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGoodd.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckFairr_Click() {
                                                            var ctl = $('#<%=CheckFairr.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPoorr.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGoodd.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckGoodd_Click() {
                                                            var ctl = $('#<%=CheckGoodd.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPoorr.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckFairr.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Inter sibling relationship :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="Check_Poor" runat="server" CssClass="checkboes" onclick="Check_Poorr_Click();" Text="Poor" />
                                                    <asp:CheckBox ID="Check_Fair" runat="server" CssClass="checkboes" onclick="Check_Fairr_Click();" Text="Fair" />
                                                    <asp:CheckBox ID="Check_Good" runat="server" CssClass="checkboes" onclick="Check_Goodd_Click();" Text="Good" />
                                                    <%--<script type="text/javascript">
                                                        function Check_Poorr_Click() {
                                                            var ctl = $('#<%=Check_Poor.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check_Fair.ClientID %>').prop('checked', false);
                                                                $('#<%=Check_Good.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckFairr_Click() {
                                                            var ctl = $('#<%=Check_Fair.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check_Poor.ClientID %>').prop('checked', false);
                                                                $('#<%=Check_Good.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Goodd_Click() {
                                                            var ctl = $('#<%=Check_Good.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check_Poor.ClientID %>').prop('checked', false);
                                                                $('#<%=Check_Fair.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Domestic violence/ Physical /mental Abuse :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckYes" runat="server" CssClass="checkboes" onclick="Check_Yes_Click();" Text="Yes" />
                                                    <asp:CheckBox ID="CheckNo" runat="server" CssClass="checkboes" onclick="Check_No_Click();" Text="No" />
                                                    <asp:CheckBox ID="CheckMaybe" runat="server" CssClass="checkboes" onclick="Check_Maybe_Click();" Text="Maybe" />
                                                    <%--<script type="text/javascript">
                                                        function Check_Yes_Click() {
                                                            var ctl = $('#<%=CheckYes.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNo.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckMaybe.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_No_Click() {
                                                            var ctl = $('#<%=CheckNo.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckYes.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckMaybe.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Maybe_Click() {
                                                            var ctl = $('#<%=CheckMaybe.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckYes.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckNo.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Family relocation :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="Check_Yes" runat="server" CssClass="checkboes" onclick="CheckYes_Click();" Text="Yes" />
                                                    <asp:CheckBox ID="Check_No" runat="server" CssClass="checkboes" onclick="CheckNo_Click();" Text="No" />
                                                    <%--<script type="text/javascript">
                                                        function CheckYes_Click() {
                                                            var ctl = $('#<%=Check_Yes.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check_No.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckNo_Click() {
                                                            var ctl = $('#<%=Check_No.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check_Yes.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                If yes, state the frequency and write the history of relocation in short :
                                            </div>
                                            <asp:TextBox ID="txtfrequency" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Primary Care giver :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckMother" runat="server" CssClass="checkboes" onclick="Check_Mother_Click();" Text="Mother" />
                                                    <asp:CheckBox ID="CheckFather" runat="server" CssClass="checkboes" onclick="Check_Father_Click();" Text="Father" />
                                                    <asp:CheckBox ID="CheckGrandparents" runat="server" CssClass="checkboes" onclick="Check_Grandparents_Click();" Text="Grandparents" />
                                                    <asp:CheckBox ID="CheckCaretaker" runat="server" CssClass="checkboes" onclick="Check_Caretaker_Click();" Text="Caretaker" />
                                                    <%--<script type="text/javascript">
                                                        function Check_Mother_Click() {
                                                            var ctl = $('#<%=CheckMother.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckFather.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGrandparents.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckCaretaker.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Father_Click() {
                                                            var ctl = $('#<%=CheckFather.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckMother.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGrandparents.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckCaretaker.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Grandparents_Click() {
                                                            var ctl = $('#<%=CheckGrandparents.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckMother.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckFather.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckCaretaker.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Caretaker_Click() {
                                                            var ctl = $('#<%=CheckCaretaker.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckMother.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckFather.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGrandparents.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's screen time :
                                            </div>
                                            <asp:TextBox ID="txtMotherScreenTime" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Screen time of the child :
                                            </div>
                                            <asp:TextBox ID="txtScreenTimeChild" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsFR" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Maternal History">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Prenatal conditions :
                                            </div>
                                            <asp:TextBox ID="txtPrenatalCondition" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Maternal Stress :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckPhysical" runat="server" CssClass="checkboes" onclick="Check_Physical_Click();" Text="Physical" />
                                                    <asp:CheckBox ID="CheckMental" runat="server" CssClass="checkboes" onclick="Check_Mental_Click();" Text="Mental" />
                                                    <%--<script type="text/javascript">
                                                        function Check_Physical_Click() {
                                                            var ctl = $('#<%=CheckPhysical.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckMental.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Mental_Click() {
                                                            var ctl = $('#<%=CheckMental.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPhysical.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Describe stressors in short :
                                            </div>
                                            <asp:TextBox ID="txtDescribeStressors" runat="server" CssClass="span10" TextMode="MultiLine"  Rows="3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Weight gain during pregnancy :
                                            </div>
                                            <asp:TextBox ID="txtWGDP" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Foetal movements :
                                            </div>
                                            <asp:TextBox ID="txtFoetalMovement" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Prenatal wellness program attended? :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckYess" runat="server" CssClass="checkboes" onclick="Check_Yes_Click();" Text="Yes" />
                                                    <asp:CheckBox ID="CheckNoo" runat="server" CssClass="checkboes" onclick="Check_No_Click();" Text="No" />
                                                    <script type="text/javascript">
                                                        function Check_Yes_Click() {
                                                            var ctl = $('#<%=CheckYess.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNoo.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_No_Click() {
                                                            var ctl = $('#<%=CheckNoo.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckYess.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsMH" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Peri and Postnatal History">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Duration of labour :
                                            </div>
                                            <asp:TextBox ID="txtDurationLabour" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Type of delivery :
                                            </div>
                                            <asp:CheckBox runat="server" ID="rdoFTND" CssClass="checkboes" Text="FTND" />
                                            <asp:CheckBox runat="server" ID="rdoFTNDva" CssClass="checkboes" Text="FTND vacuum assisted" />
                                            <asp:CheckBox runat="server" ID="rdoELSCS" CssClass="checkboes" Text="E- LSCS" />
                                            <asp:CheckBox runat="server" ID="rdoElectiveLSCS" CssClass="checkboes" Text="Elective LSCS" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                CIAB? :
                                            </div>
                                            <asp:CheckBox runat="server" ID="rdoYess" CssClass="checkboes" Text="Yes" onclick="rdoYess_check()" />
                                            <asp:CheckBox runat="server" ID="rdoNoo" CssClass="checkboes" Text="No" onclick="rdoNoo_check()" />
                                            <script type="text/javascript">
                                                function rdoYess_check() {
                                                    var ctl = $('#<%=rdoYess.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=rdoNoo.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function rdoNoo_check() {
                                                    var ctl = $('#<%=rdoNoo.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=rdoYess.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Conditions post birth :
                                            </div>
                                            <asp:TextBox ID="txtConditionPostBirth" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Birth Weight :
                                            </div>
                                            <asp:TextBox ID="txtBirthWeight" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Gestational Birth Age :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RdoAGA" CssClass="checkboes" Text="AGA" />
                                            <asp:CheckBox runat="server" ID="RdoSGA" CssClass="checkboes" Text="SGA" />
                                            <asp:CheckBox runat="server" ID="RdoLGA" CssClass="checkboes" Text="LGA" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                NICU stay :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RdoPresent" CssClass="checkboes" Text="Present" onclick="RdoPresent_check()" />
                                            <asp:CheckBox runat="server" ID="RdoAbsent" CssClass="checkboes" Text="Absent" onclick="RdoAbsent_check()" />
                                            <script type="text/javascript">
                                                function RdoPresent_check() {
                                                    var ctl = $('#<%=RdoPresent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RdoAbsent.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RdoAbsent_check() {
                                                    var ctl = $('#<%=RdoAbsent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RdoPresent.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Duration of the NICU stay :
                                            </div>
                                            <asp:TextBox ID="txtDurationNICUstay" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                NICU History :
                                            </div>
                                            <asp:TextBox ID="txtNICUHistory" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Reason For NICU stay :
                                            </div>
                                            <asp:TextBox ID="txtReasonNICUstay" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                APGAR score  :
                                            </div>
                                            <asp:TextBox ID="txtAPGARscore" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Breast fed :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RdoYes" CssClass="checkboes" Text="Yes" onclick="RdoYes_check()" />
                                            <asp:CheckBox runat="server" ID="RdoNo" CssClass="checkboes" Text="No" onclick="RdoNo_check()" />
                                            <script type="text/javascript">
                                                function RdoYes_check() {
                                                    var ctl = $('#<%=RdoYes.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RdoNo.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RdoNo_check() {
                                                    var ctl = $('#<%=RdoNo.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RdoYes.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                If not, how was the baby fed  :
                                            </div>
                                            <asp:TextBox ID="txtBabyFed" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Problems during breast feeding  :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioPresent" CssClass="checkboes" Text="Present" onclick="RadioPresent_check()" />
                                            <asp:CheckBox runat="server" ID="RadioAbsent" CssClass="checkboes" Text="Absent" onclick="RadioAbsent_check()" />
                                            <script type="text/javascript">
                                                function RadioPresent_check() {
                                                    var ctl = $('#<%=RadioPresent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioAbsent.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioAbsent_check() {
                                                    var ctl = $('#<%=RadioAbsent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioPresent.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mention problems :
                                            </div>
                                            <asp:TextBox ID="txtMentionProblem" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Till what age was the child breast fed? (If child more than 1.5 years old ) :
                                            </div>
                                            <asp:TextBox ID="txtwaswtcbf" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Any colic issues as a baby? :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioYes" CssClass="checkboes" Text="Yes" onclick="RadioYes_check()" />
                                            <asp:CheckBox runat="server" ID="RadioNo" CssClass="checkboes" Text="No" onclick="RadioNo_check()" />
                                            <script type="text/javascript">
                                                function RadioYes_check() {
                                                    var ctl = $('#<%=RadioYes.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioNo.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioNo_check() {
                                                    var ctl = $('#<%=RadioNo.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioYes.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Other medical issues :
                                            </div>
                                            <asp:TextBox ID="txtOthrtMedicalIssues" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsPPH" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="Developmental Milestones">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Gross Motor :
                                            </div>
                                            <asp:TextBox ID="txtGrossMotor" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Fine Motor  :
                                            </div>
                                            <asp:TextBox ID="txtFineMotor" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Personal and Social :
                                            </div>
                                            <asp:TextBox ID="txtPersonalandSocial" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Communication  :
                                            </div>
                                            <asp:TextBox ID="txtCommunication" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments  :
                                            </div>
                                            <asp:TextBox ID="txtCommentsDM" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel7" runat="server" HeaderText="Sleep">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Sleep issues during 0-6 months (put NA if not relevant) :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadiooNo" CssClass="checkboes" Text="No" onclick="RadiooNo_check()" />
                                            <asp:CheckBox runat="server" ID="RadiooYes" CssClass="checkboes" Text="Yes" onclick="RadiooYes_check()" />
                                            <script type="text/javascript">
                                                function RadiooNo_check() {
                                                    var ctl = $('#<%=RadiooNo.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadiooYes.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadiooYes_check() {
                                                    var ctl = $('#<%=RadiooYes.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadiooNo.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Present sleep concerns :
                                            </div>
                                            <asp:CheckBox runat="server" ID="PresentRadio" CssClass="checkboes" Text="Present" onclick="PresentRadio_check()" />
                                            <asp:CheckBox runat="server" ID="AbsentRadio" CssClass="checkboes" Text="Absent" onclick="AbsentRadio_check()" />
                                            <script type="text/javascript">
                                                function PresentRadio_check() {
                                                    var ctl = $('#<%=PresentRadio.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=AbsentRadio.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function AbsentRadio_check() {
                                                    var ctl = $('#<%=AbsentRadio.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=PresentRadio.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Sleep duration :
                                            </div>
                                            <asp:TextBox ID="txtSleepduration" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Sleep Type :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioLight" CssClass="checkboes" Text="Light" onclick="RadioLight_click()" />
                                            <asp:CheckBox runat="server" ID="RadioDeep" CssClass="checkboes" Text="Deep" onclick="RadioDeep_check()" />
                                            <script type="text/javascript">
                                                function RadioLight_click() {
                                                    var ctl = $('#<%=RadioLight.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioDeep.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioDeep_check() {
                                                    var ctl = $('#<%=RadioDeep.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioLight.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Co-sleeping :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioAbsentbtn" CssClass="checkboes" Text="Absent" onclick="RadioAbsentbtn_click()" />
                                            <asp:CheckBox runat="server" ID="RadioPresentbtn" CssClass="checkboes" Text="Present" onclick="RadioPresentbtn_click()" />
                                            <script type="text/javascript">
                                                function RadioAbsentbtn_click() {
                                                    var ctl = $('#<%=RadioAbsentbtn.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioPresentbtn.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioPresentbtn_click() {
                                                    var ctl = $('#<%=RadioPresentbtn.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioAbsentbtn.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Co- sleeping with ? :
                                            </div>
                                            <asp:TextBox ID="txtCosleepingwith" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Any Sleep Adjuncts used ? :
                                            </div>
                                            <asp:TextBox ID="txtAnySleepAdjunctsused" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Nap time :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioButtonPresent" CssClass="checkboes" Text="Present" onclick="RadioButtonPresent_click()" />
                                            <asp:CheckBox runat="server" ID="RadioButtonAbsent" CssClass="checkboes" Text="Absent" onclick="RadioButtonAbsent_click()" />
                                            <script type="text/javascript">
                                                function RadioButtonPresent_click() {
                                                    var ctl = $('#<%=RadioButtonPresent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioButtonAbsent.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioButtonAbsent_click() {
                                                    var ctl = $('#<%=RadioButtonAbsent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioButtonPresent.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Nap duration  :
                                            </div>
                                            <asp:TextBox ID="txtNapduration" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsS" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel8" runat="server" HeaderText="Feeding Habits">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Feeding habits  :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioTypical" CssClass="checkboes" Text="Typical" onclick="RadioTypical_check()" />
                                            <asp:CheckBox runat="server" ID="RadioAtypical" CssClass="checkboes" Text="Atypical" onclick="RadioAtypical_check()" />
                                            <script type="text/javascript">
                                                function RadioTypical_check() {
                                                    var ctl = $('#<%=RadioTypical.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioAtypical.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioAtypical_check() {
                                                    var ctl = $('#<%=RadioAtypical.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioTypical.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Type of food had  :
                                            </div>
                                            <asp:TextBox ID="txtTypeoffoodhad" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Food consistency :
                                            </div>
                                            <asp:TextBox ID="txtFoodconsistency" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Food temperature :
                                            </div>
                                            <asp:TextBox ID="txtFoodtemperature" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Food taste :
                                            </div>
                                            <asp:TextBox ID="txtFoodtaste" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsFeHa" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel9" runat="server" HeaderText="Into the child’s heart">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                What are your child’s likes and dislikes ? :
                                            </div>
                                            <asp:TextBox ID="txtChildLikes" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--<div class="formRow"> 
                                        <div class="span12">
                                            <div class="control-label">
                                                What are your child’s dislikes? :
                                            </div>
                                            <asp:TextBox ID="txtChildDislikes" runat="server" CssClass="span4" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                What are his/her moments of happiness? :
                                            </div>
                                            <asp:TextBox ID="txtMomentsOfHappiness" runat="server" CssClass="span4" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                What are his/her moments of fear? :
                                            </div>
                                            <asp:TextBox ID="txtMomentsOfFear" runat="server" CssClass="span4" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Can your child show/describe his/her feelings and emotions? :
                                            </div>
                                            <asp:TextBox ID="txtFeelingsNemotions" runat="server" CssClass="span4" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Do you think your child shows signs of stress/ anxiety ? :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioButtonYes" CssClass="checkboes" Text="Yes" />
                                            <asp:CheckBox runat="server" ID="RadioButtonNo" CssClass="checkboes" Text="No" />
                                            <asp:CheckBox runat="server" ID="RadioButtonMaybe" CssClass="checkboes" Text="Maybe" />
                                        </div>
                                    </div>--%>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsITCH" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel10" runat="server" HeaderText="Play Behaviour">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Play behaviour :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioOrganised" CssClass="checkboes" Text="Organised" onclick="RadioOrganised_check()" />
                                            <asp:CheckBox runat="server" ID="RadioDisorganised" CssClass="checkboes" Text="Disorganised" onclick="RadioDisorganised_check()" />
                                            <script type="text/javascript">
                                                function RadioOrganised_check() {
                                                    var ctl = $('#<%=RadioOrganised.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioDisorganised.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioDisorganised_check() {
                                                    var ctl = $('#<%=RadioDisorganised.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioOrganised.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Interaction with peers  :
                                            </div>
                                            <asp:TextBox ID="txtInteractionwithpeers" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Stranger anxiety ? :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioPresentButton" CssClass="checkboes" Text="Present" onclick="RadioPresentButton_check()" />
                                            <asp:CheckBox runat="server" ID="RadioAbsentButton" CssClass="checkboes" Text="Absent" onclick="RadioAbsentButton_check()" />
                                            <script type="text/javascript">
                                                function RadioPresentButton_check() {
                                                    var ctl = $('#<%=RadioPresentButton.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioAbsentButton.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioAbsentButton_check() {
                                                    var ctl = $('#<%=RadioAbsentButton.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioPresentButton.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Does your child play with toys? :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioYesButton" CssClass="checkboes" Text="Yes" onclick="RadioYesButton_check()" />
                                            <asp:CheckBox runat="server" ID="RadioNoButton" CssClass="checkboes" Text="No" onclick="RadioNoButton_check()" />
                                            <asp:CheckBox runat="server" ID="RadioMaybeButton" CssClass="checkboes" Text="Maybe" onclick="RadioMaybeButton_check()" />
                                            <script type="text/javascript">
                                                function RadioYesButton_check() {
                                                    var ctl = $('#<%=RadioYesButton.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioNoButton.ClientID %>').prop('checked', false);
                                                        $('#<%=RadioMaybeButton.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioNoButton_check() {
                                                    var ctl = $('#<%=RadioNoButton.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioYesButton.ClientID %>').prop('checked', false);
                                                        $('#<%=RadioMaybeButton.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioMaybeButton_check() {
                                                    var ctl = $('#<%=RadioMaybeButton.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioYesButton.ClientID %>').prop('checked', false);
                                                        $('#<%=RadioNoButton.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Preference of toys :
                                            </div>
                                            <asp:TextBox ID="txtPreferenceoftoys" runat="server" CssClass="span4 savedata"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsPB" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel11" runat="server" HeaderText="ADL's">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Brushing :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioDependent" CssClass="checkboes" Text="Dependent" />
                                            <asp:CheckBox runat="server" ID="RadioAssisted" CssClass="checkboes" Text="Assisted" />
                                            <asp:CheckBox runat="server" ID="RadioIndependent" CssClass="checkboes" Text="Independent" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsBrushing" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Bathing :
                                            </div>
                                            <asp:CheckBox runat="server" ID="DependentRadio" CssClass="checkboes" Text="Dependent" />
                                            <asp:CheckBox runat="server" ID="AssistedRadio" CssClass="checkboes" Text="Assisted" />
                                            <asp:CheckBox runat="server" ID="IndependentRadio" CssClass="checkboes" Text="Independent" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsBathing" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Toileting :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioDependentButton" CssClass="checkboes" Text="Dependent" />
                                            <asp:CheckBox runat="server" ID="RadioAssistedButton" CssClass="checkboes" Text="Assisted" />
                                            <asp:CheckBox runat="server" ID="RadioIndependentButton" CssClass="checkboes" Text="Independent" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsToileting" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Dressing :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioButtonDependent" CssClass="checkboes" Text="Dependent" />
                                            <asp:CheckBox runat="server" ID="RadioButtonAssisted" CssClass="checkboes" Text="Assisted" />
                                            <asp:CheckBox runat="server" ID="RadioButtonIndependent" CssClass="checkboes" Text="Independent" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsDressing" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Eating :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioBtnDependent" CssClass="checkboes" Text="Dependent" />
                                            <asp:CheckBox runat="server" ID="RadioBtnAssisted" CssClass="checkboes" Text="Assisted" />
                                            <asp:CheckBox runat="server" ID="RadioBtnIndependent" CssClass="checkboes" Text="Independent" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsEating" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Ambulation  :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RdoDependent" CssClass="checkboes" Text="Dependent" />
                                            <asp:CheckBox runat="server" ID="RdoAssisted" CssClass="checkboes" Text="Assisted" />
                                            <asp:CheckBox runat="server" ID="RdoIndependent" CssClass="checkboes" Text="Independent" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsAmbulation" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Transfers   :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RdobtnDependent" CssClass="checkboes" Text="Dependent" />
                                            <asp:CheckBox runat="server" ID="RdobtnAssisted" CssClass="checkboes" Text="Assisted" />
                                            <asp:CheckBox runat="server" ID="RdobtnIndependent" CssClass="checkboes" Text="Independent" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsTransfers" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel14" runat="server" HeaderText="Observations">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Add comments :
                                            </div>
                                            <asp:TextBox ID="txtAddComments" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Evaluation Recommended">
                     <ContentTemplate>
                         <div style="margin-top: 20px; margin-bottom: 20px;">
                             <div class="formRow">
                                 <div class="span12">
                                     <div class="control-label">
                                         Add comments :
                                     </div>
                                     <asp:TextBox ID="txtAddEvalRec" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                 </div>
                             </div>
                             <div class="clearfix"></div>
                         </div>
                     </ContentTemplate>
                 </ajaxToolkit:TabPanel>
             </ajaxToolkit:TabContainer>
             <div class="clearfix">
                <button type="button" id="btnSaveNext" class="buttonClass" style="width:200px;display:none">
    Save&Next
     </button>
                    </div>
                    
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="clearfix"></div>
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
            <h5 class="modal-title">YOU DID NOT CLICK ON SAVE&NEXT BUTTON DATA IS SAVING PLEASE WAIT.. </h5>
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
    
      <script type="text/javascript">
          var preTabId = "";
          var CurTabId = "";
          var nextAfterSave = false;
          let saveInProgress = false;
          $(document).ready(function () {

              if (!$("#hfdPrevTab").val()) $("#hfdPrevTab").val("TabPanel13");
              if (!$("#hfdCurTab").val()) $("#hfdCurTab").val("TabPanel13");

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
          $(document).on('click', '.addRow', function () {

              var row = $(this).closest('.cloneThisRow');
              var clone = row.clone(false);   // ❗ do NOT clone events

              // clear text fields
              clone.find("textarea, input[type='text']").val("");

              // VERY IMPORTANT: reset hidden ID
              clone.find("input[id*='txtPreConsultID']").val("0");

              row.after(clone);
          });
          function clientActiveTabChanged(sender, args) {

              try {
                  var tab = sender.get_tabs()[sender.get_activeTabIndex()];
                  CurTabId = tab.get_id();

                  var prevTab = $("#hfdCurTab").val();

                  $("#hfdPrevTab").val(prevTab);
                  $("#hfdCurTab").val(CurTabId);

                  if (prevTab && prevTab !== CurTabId) {
                      SaveTabById(prevTab, false);
                  }

              } catch (ex) {
                  console.log("clientActiveTabChanged error:", ex);
              }
          }

          function getCurrentTabId() {
              var cur = $("#hfdCurTab").val();
              if (!cur || cur === "undefined") cur = "TabPanel13";
              return cur;
          }

          function getPreviousTabId() {
              var prev = $("#hfdPrevTab").val();
              if (!prev || prev === "undefined") prev = "TabPanel13";
              return prev;
          }

          function SaveTabById(tabId, reloadAfterSave) {
              if (saveInProgress) {
                  console.warn("Save blocked: already in progress");
                  return;
              }

              switch (tabId) {

                  case "TabPanel13": SaveTab1(reloadAfterSave); break;
                  case "TabPanel1": SaveTab2(reloadAfterSave); break;
                  case "TabPanel0": SaveTab3(reloadAfterSave); break;
                  case "TabPanel2": SaveTab4(reloadAfterSave); break;
                  case "TabPanel4": SaveTab5(reloadAfterSave); break;
                  case "TabPanel5": SaveTab6(reloadAfterSave); break;
                  case "TabPanel6": SaveTab7(reloadAfterSave); break;
                  case "TabPanel7": SaveTab8(reloadAfterSave); break;
                  case "TabPanel8": SaveTab9(reloadAfterSave); break;
                  case "TabPanel9": SaveTab10(reloadAfterSave); break;
                  case "TabPanel10": SaveTab11(reloadAfterSave); break;
                  case "TabPanel11": SaveTab12(reloadAfterSave); break;
                  case "TabPanel14": SaveTab13(reloadAfterSave); break;
                  case "TabPanel3": SaveTab14(reloadAfterSave); break;

                  default:
                      console.log("No save function for tab:", tabId);
                      break;
              }
          }

          function PostToHandler(formData, reloadAfterSave, tabName) {
              if (saveInProgress) return;

              saveInProgress = true;
              $("#btnSaveNext").prop("disabled", true);
              $("#btnFinalSubmit").prop("disabled", true);
              $.ajax({
                  type: "POST",
                  url: "<%= ResolveUrl("~/Handler/PreconstRpt_2021.ashx") %>",
                 data: formData,
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
          function SaveTab1(reloadAfterSave) {

              var formData = {};
              formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 1;

             formData.ComfortableLanguage = $("#<%= txtComfortableLanguage.ClientID %>").val();
             formData.DatepreConsult = $("#<%= txtDatepreConsult.ClientID %>").val();
             formData.DateBirth = $("#<%= txtDateBirth.ClientID %>").val();
             formData.DateDelivery = $("#<%= txtDateofDelivery.ClientID %>").val();

             formData.CorrectAge = $("#<%= txtCorrectAge.ClientID %>").val();
             formData.Age = $("#<%= txtAge.ClientID %>").val();

             // Gender
             var gender = "";
             if ($("#<%= CheckFemale.ClientID %>").is(":checked")) gender = "Female";
             else if ($("#<%= CheckMale.ClientID %>").is(":checked")) gender = "Male";
             else if ($("#<%= CheckOther.ClientID %>").is(":checked")) gender = "Other";
             formData.Gender = gender;

             // Child Attend
             var childAttend = "";
             if ($("#<%= YesAttend.ClientID %>").is(":checked")) childAttend = "Yes";
             else if ($("#<%= Noattend.ClientID %>").is(":checked")) childAttend = "No";
             formData.ChildAttend = childAttend;

             formData.OnlineOffline = $("#<%= txtOnlineOffline.ClientID %>").val();
             formData.WhichGrade = $("#<%= txtWhichGrade.ClientID %>").val();

             formData.MotherName = $("#<%= txtMotherName.ClientID %>").val();
             formData.MotherAge = $("#<%= txtMotherAge.ClientID %>").val();
             formData.MotherQualification = $("#<%= txtMotherQualification.ClientID %>").val();
             formData.MotherOccupation = $("#<%= txtMotherOccupation.ClientID %>").val();
             formData.MotherWorkingHour = $("#<%= txtMotherWorkingHour.ClientID %>").val();

             formData.FatherName = $("#<%= txtFatherName.ClientID %>").val();
             formData.FatherAge = $("#<%= txtFatherAge.ClientID %>").val();
             formData.FatherOccupation = $("#<%= txtFatherOccupation.ClientID %>").val();
             formData.FatherQualification = $("#<%= txtFatherQualification.ClientID %>").val();
             formData.FatherWorkingHour = $("#<%= txtFatherWorkingHour.ClientID %>").val();

             formData.Address = $("#<%= txtAddress.ClientID %>").val();
             formData.ContactDetails = $("#<%= txtContactDetails.ClientID %>").val();
             formData.EmailID = $("#<%= txtEmailID.ClientID %>").val();
             formData.ReferredBy = $("#<%= txtReferredBy.ClientID %>").val();
             formData.TherapistDuringPC = $("#<%= txtTherapistDuringPC.ClientID %>").val();
             formData.Diagnosis = $("#<%= txtDiagnosis.ClientID %>").val();
             formData.CommentsPI = $("#<%= txtCommentsPI.ClientID %>").val();

              showConfirmPopup(formData, function () {
                  PostToHandler(formData, reloadAfterSave, "Patient Information");
              });
          }
          function SaveTab2(reloadAfterSave) {

              var formData = {};
              formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 2;

             formData.ChiefConcernsHome = $("#<%= txtChiefConcernsHome.ClientID %>").val();
             formData.ChiefConcernsSchool = $("#<%= txtChiefConcernsSchool.ClientID %>").val();
             formData.ChiefConcernsSocialGath = $("#<%= txtChiefConcernsSocialGath.ClientID %>").val();
             formData.CommentsCC = $("#<%= txtCommentsCC.ClientID %>").val();

              showConfirmPopup(formData, function () {
                  PostToHandler(formData, reloadAfterSave, "Chief Concerns");
              });
          }
          function SaveTab3(reloadAfterSave) {

              var formData = {};
              formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 3;

             var timelineData = "";
             var orderNo = 1;

             $(".cloneThisRow:not(.hide)").each(function () {

                 var row = $(this);

                 var preConsultID = row.find("input[id*='txtPreConsultID']").val() || "0";

                 var DateMonth = row.find("textarea[id*='txtDateMonth']").val() || "";
                 var RelevantHistory = row.find("textarea[id*='txtRelevantHistory']").val() || "";
                 var HospitalDoctorsVisited = row.find("textarea[id*='txtHospitalDoctorsVisited']").val() || "";
                 var DoctorsRecommendations = row.find("textarea[id*='txtDoctorsRecommendations']").val() || "";
                 var InvestigationsRecordsResults = row.find("textarea[id*='txtInvestigationsRecordsResults']").val() || "";

                 if (
                     DateMonth.trim() === "" &&
                     RelevantHistory.trim() === "" &&
                     HospitalDoctorsVisited.trim() === "" &&
                     DoctorsRecommendations.trim() === "" &&
                     InvestigationsRecordsResults.trim() === ""
                 ) {
                     return;
                 }

                 DateMonth = DateMonth.replace(/[#\$~|]/g, " ");
                 RelevantHistory = RelevantHistory.replace(/[#\$~|]/g, " ");
                 HospitalDoctorsVisited = HospitalDoctorsVisited.replace(/[#\$~|]/g, " ");
                 DoctorsRecommendations = DoctorsRecommendations.replace(/[#\$~|]/g, " ");
                 InvestigationsRecordsResults = InvestigationsRecordsResults.replace(/[#\$~|]/g, " ");

                 // 👇 ORDER APPENDED AT END
                 timelineData += preConsultID + "#"
                     + DateMonth + "$"
                     + RelevantHistory + "$"
                     + HospitalDoctorsVisited + "$"
                     + DoctorsRecommendations + "$"
                     + InvestigationsRecordsResults
                     + "|" + orderNo + "~";

                 orderNo++;
             });

             if (timelineData.endsWith("~")) {
                 timelineData = timelineData.slice(0, -1);
             }

             formData.TimelineData = timelineData;
             formData.DeletedTimelineIds =
                 $('#<%= hdnDeletedTimelineIds.ClientID %>').val();

              showConfirmPopup(formData, function () {
                  PostToHandler(formData, reloadAfterSave, "Timeline");
              });
          }

          function SaveTab4(reloadAfterSave) {

              var formData = {};
              formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 4;

             // Consanguinity
             formData.Consanguinity = $("#<%= CheckConsan.ClientID %>").is(":checked") ? "Consanguineous Marriage" : "";
             formData.Consanguinity_1 = $("#<%= CheckNonConsan.ClientID %>").is(":checked") ? "Non- Consanguineous" : "";

             // Degree of Consanguinity
             formData.ConsanguinityDegree = $("#<%= Check1Deg.ClientID %>").is(":checked") ? "1st deg" : "";
             formData.ConsanguinityDegree_1 = $("#<%= Check2Deg.ClientID %>").is(":checked") ? "2nd deg" : "";
             formData.ConsanguinityDegree_2 = $("#<%= Check3Deg.ClientID %>").is(":checked") ? "3rd deg" : "";

             // Years of marriage
             formData.YearsMarriage = $("#<%= txtYearsMarriage.ClientID %>").val();

             // Family Structure
             formData.FamilyStructure = $("#<%= CheckNuclear.ClientID %>").is(":checked") ? "Nuclear" : "";
             formData.FamilyStructure_1 = $("#<%= CheckJoint.ClientID %>").is(":checked") ? "Joint" : "";

             // Conception
             formData.Conception = $("#<%= CheckNatural.ClientID %>").is(":checked") ? "Natural" : "";
             formData.Conception_1 = $("#<%= CheckIUI.ClientID %>").is(":checked") ? "IUI" : "";
             formData.Conception_2 = $("#<%= CheckIVF.ClientID %>").is(":checked") ? "IVF" : "";
             formData.Conception_3 = $("#<%= CheckISCI.ClientID %>").is(":checked") ? "ISCI" : "";
             formData.Conception_4 = $("#<%= CheckOI.ClientID %>").is(":checked") ? "OI" : "";

             // Planning of Conception
             formData.PlanningConception = $("#<%= CheckPlanned.ClientID %>").is(":checked") ? "Planned" : "";
             formData.PlanningConception_1 = $("#<%= CheckUnplanned.ClientID %>").is(":checked") ? "Unplanned" : "";

             // Siblings
             formData.Siblings = $("#<%= AnySiblingsYes.ClientID %>").is(":checked") ? "Yes" :
                 $("#<%= AnySiblingsNo.ClientID %>").is(":checked") ? "No" : "";

             formData.NoOfSiblings = $("#<%= txtNoOfSiblings.ClientID %>").val();
             formData.RHASiblings = $("#<%= txtRHASiblings.ClientID %>").val();
             formData.CommentsFH = $("#<%= txtCommentsFH.ClientID %>").val();

             // Inter parental relationship
             formData.InterParentalRelation = $("#<%= CheckPoor.ClientID %>").is(":checked") ? "Poor" : "";
             formData.InterParentalRelation_1 = $("#<%= CheckFair.ClientID %>").is(":checked") ? "Fair" : "";
             formData.InterParentalRelation_2 = $("#<%= CheckGood.ClientID %>").is(":checked") ? "Good" : "";

             // Parent child relationship
             formData.ParentChildRelation = $("#<%= CheckPoorr.ClientID %>").is(":checked") ? "Poor" : "";
             formData.ParentChildRelation_1 = $("#<%= CheckFairr.ClientID %>").is(":checked") ? "Fair" : "";
             formData.ParentChildRelation_2 = $("#<%= CheckGoodd.ClientID %>").is(":checked") ? "Good" : "";

             formData.InterSiblingRelation = $("#<%= Check_Poor.ClientID %>").is(":checked") ? "Poor" : "";
             formData.InterSiblingRelation_1 = $("#<%= Check_Fair.ClientID %>").is(":checked") ? "Fair" : "";
             formData.InterSiblingRelation_2 = $("#<%= Check_Good.ClientID %>").is(":checked") ? "Good" : "";

             formData.DomesticViolence = $("#<%= CheckYes.ClientID %>").is(":checked") ? "Yes" : "";
             formData.DomesticViolence_1 = $("#<%= CheckNo.ClientID %>").is(":checked") ? "No" : "";
             formData.DomesticViolence_2 = $("#<%= CheckMaybe.ClientID %>").is(":checked") ? "Maybe" : "";

             formData.FamilyRelocation = $("#<%= Check_Yes.ClientID %>").is(":checked") ? "Yes" : "";
             formData.FamilyRelocation_1 = $("#<%= Check_No.ClientID %>").is(":checked") ? "No" : "";

             formData.frequency = $("#<%= txtfrequency.ClientID %>").val();
             formData.PrimaryCare = $("#<%= CheckMother.ClientID %>").is(":checked") ? "Mother" : "";
             formData.PrimaryCare_1 = $("#<%= CheckFather.ClientID %>").is(":checked") ? "Father" : "";
             formData.PrimaryCare_2 = $("#<%= CheckGrandparents.ClientID %>").is(":checked") ? "Grandparents" : "";
             formData.PrimaryCare_3 = $("#<%= CheckCaretaker.ClientID %>").is(":checked") ? "Caretaker" : "";

             formData.MotherScreenTime = $("#<%= txtMotherScreenTime.ClientID %>").val();
             formData.ScreenTimeChild = $("#<%= txtScreenTimeChild.ClientID %>").val();
             formData.CommentsFR = $("#<%= txtCommentsFR.ClientID %>").val();

              showConfirmPopup(formData, function () {
                  PostToHandler(formData, reloadAfterSave, "Family History and Relations");
              });
          }
          function SaveTab5(reloadAfterSave) {

              var formData = {};
              formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 5;

             formData.PrenatalCondition = $("#<%= txtPrenatalCondition.ClientID %>").val();

             formData.MaternalStress = $("#<%= CheckPhysical.ClientID %>").is(":checked") ? "Physical" : "";
             formData.MaternalStress_1 = $("#<%= CheckMental.ClientID %>").is(":checked") ? "Mental" : "";

             formData.DescribeStressors = $("#<%= txtDescribeStressors.ClientID %>").val();
             formData.WGDP = $("#<%= txtWGDP.ClientID %>").val();
             formData.FoetalMovement = $("#<%= txtFoetalMovement.ClientID %>").val();

             formData.Prenatalwellness = $("#<%= CheckYess.ClientID %>").is(":checked") ? "Yes" :
                 $("#<%= CheckNoo.ClientID %>").is(":checked") ? "No" : "";

             formData.CommentsMH = $("#<%= txtCommentsMH.ClientID %>").val();

              showConfirmPopup(formData, function () {
                  PostToHandler(formData, reloadAfterSave, "Maternal History");
              });

          }
          function SaveTab6(reloadAfterSave) {

              var formData = {};
              formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 6;

             formData.DurationLabour = $("#<%= txtDurationLabour.ClientID %>").val();

             // Type of delivery (4 checkboxes)
             formData.delivery = $("#<%= rdoFTND.ClientID %>").is(":checked") ? "FTND" : "";
             formData.delivery_1 = $("#<%= rdoFTNDva.ClientID %>").is(":checked") ? "FTND vacuum assisted" : "";
             formData.delivery_2 = $("#<%= rdoELSCS.ClientID %>").is(":checked") ? "E- LSCS" : "";
             formData.delivery_3 = $("#<%= rdoElectiveLSCS.ClientID %>").is(":checked") ? "Elective LSCS" : "";

             // CIAB (Yes/No)
             formData.ciab = $("#<%= rdoYess.ClientID %>").is(":checked") ? "Yes" :
                 $("#<%= rdoNoo.ClientID %>").is(":checked") ? "No" : "";

             formData.ConditionPostBirth = $("#<%= txtConditionPostBirth.ClientID %>").val();
             formData.BirthWeight = $("#<%= txtBirthWeight.ClientID %>").val();

             // Gestational Birth Age (AGA/SGA/LGA)
             formData.GestationalBirthAge = $("#<%= RdoAGA.ClientID %>").is(":checked") ? "AGA" : "";
             formData.GestationalBirthAge_1 = $("#<%= RdoSGA.ClientID %>").is(":checked") ? "SGA" : "";
             formData.GestationalBirthAge_2 = $("#<%= RdoLGA.ClientID %>").is(":checked") ? "LGA" : "";

             // NICU stay (Present/Absent)
             formData.NICUstay = $("#<%= RdoPresent.ClientID %>").is(":checked") ? "Present" :
                 $("#<%= RdoAbsent.ClientID %>").is(":checked") ? "Absent" : "";

              formData.DurationNICUstay = $("#<%= txtDurationNICUstay.ClientID %>").val();
              formData.NICUHistory = $("#<%= txtNICUHistory.ClientID %>").val();
              formData.ReasonNICUstay = $("#<%= txtReasonNICUstay.ClientID %>").val();
              formData.APGARscore = $("#<%= txtAPGARscore.ClientID %>").val();
             
              // Breast fed (Yes/No)
              formData.Breastfed = $("#<%= RdoYes.ClientID %>").is(":checked") ? "Yes" :
                                   $("#<%= RdoNo.ClientID %>").is(":checked") ? "No" : "";
             
              formData.BabyFed = $("#<%= txtBabyFed.ClientID %>").val();
             
              // Problems during breastfeeding (Present/Absent)
              formData.Problemsduringbreastfeeding = $("#<%= RadioPresent.ClientID %>").is(":checked") ? "Present" :
                                                    $("#<%= RadioAbsent.ClientID %>").is(":checked") ? "Absent" : "";
             
              formData.MentionProblem = $("#<%= txtMentionProblem.ClientID %>").val();
              formData.waswtcbf = $("#<%= txtwaswtcbf.ClientID %>").val();
             
              // colic issue (Yes/No)
              formData.colicissue = $("#<%= RadioYes.ClientID %>").is(":checked") ? "Yes" :
                                    $("#<%= RadioNo.ClientID %>").is(":checked") ? "No" : "";
             
              formData.OthrtMedicalIssues = $("#<%= txtOthrtMedicalIssues.ClientID %>").val();
             formData.CommentsPPH = $("#<%= txtCommentsPPH.ClientID %>").val();

             showConfirmPopup(formData, function () {
                 PostToHandler(formData, reloadAfterSave, "Peri and Postnatal History");
             });
         }
         function SaveTab7(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 7;

             formData.GrossMotor = $("#<%= txtGrossMotor.ClientID %>").val();
             formData.FineMotor = $("#<%= txtFineMotor.ClientID %>").val();
             formData.PersonalandSocial = $("#<%= txtPersonalandSocial.ClientID %>").val();
             formData.Communication = $("#<%= txtCommunication.ClientID %>").val();
             formData.CommentsDM = $("#<%= txtCommentsDM.ClientID %>").val();

             showConfirmPopup(formData, function () {
                 PostToHandler(formData, reloadAfterSave, "Developmental Milestones");
             });
         }
         function SaveTab8(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 8;

             formData.Sleepissues = $("#<%= RadiooYes.ClientID %>").is(":checked") ? "Yes" :
                 $("#<%= RadiooNo.ClientID %>").is(":checked") ? "No" : "";

             formData.Presentsleep = $("#<%= PresentRadio.ClientID %>").is(":checked") ? "Present" :
                 $("#<%= AbsentRadio.ClientID %>").is(":checked") ? "Absent" : "";

             formData.Sleepduration = $("#<%= txtSleepduration.ClientID %>").val();
             formData.SleepType = $("#<%= RadioLight.ClientID %>").is(":checked") ? "Light" :
                 $("#<%= RadioDeep.ClientID %>").is(":checked") ? "Deep" : "";
              formData.Cosleeping = $("#<%= RadioPresentbtn.ClientID %>").is(":checked") ? "Present" :
                                    $("#<%= RadioAbsentbtn.ClientID %>").is(":checked") ? "Absent" : "";
             
              formData.Cosleepingwith = $("#<%= txtCosleepingwith.ClientID %>").val();
              formData.AnySleepAdjunctsused = $("#<%= txtAnySleepAdjunctsused.ClientID %>").val();
             
              formData.Naptime = $("#<%= RadioButtonPresent.ClientID %>").is(":checked") ? "Present" :
                                 $("#<%= RadioButtonAbsent.ClientID %>").is(":checked") ? "Absent" : "";
             
              formData.Napduration = $("#<%= txtNapduration.ClientID %>").val();
             formData.CommentsS = $("#<%= txtCommentsS.ClientID %>").val();

             showConfirmPopup(formData, function () {
                 PostToHandler(formData, reloadAfterSave, "Sleep");
             });
         }
         function SaveTab9(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 9;

             // Feeding habits (Typical / Atypical)
             formData.Feedinghabits = $("#<%= RadioTypical.ClientID %>").is(":checked") ? "Typical" :
                 $("#<%= RadioAtypical.ClientID %>").is(":checked") ? "Atypical" : "";

             formData.Typeoffoodhad = $("#<%= txtTypeoffoodhad.ClientID %>").val();
             formData.Foodconsistency = $("#<%= txtFoodconsistency.ClientID %>").val();
             formData.Foodtemperature = $("#<%= txtFoodtemperature.ClientID %>").val();
             formData.Foodtaste = $("#<%= txtFoodtaste.ClientID %>").val();
             formData.CommentsFeHa = $("#<%= txtCommentsFeHa.ClientID %>").val();

             showConfirmPopup(formData, function () {
                 PostToHandler(formData, reloadAfterSave, "Feeding Habits");
             });
         }
         function SaveTab10(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 10;

             formData.ChildLikes = $("#<%= txtChildLikes.ClientID %>").val();
             formData.CommentsITCH = $("#<%= txtCommentsITCH.ClientID %>").val();

             showConfirmPopup(formData, function () {
                 PostToHandler(formData, reloadAfterSave, "Into the child’s heart");
             });

         }
         function SaveTab11(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 11;

             // Play behaviour (Organised / Disorganised)
             formData.Playbehaviour = $("#<%= RadioOrganised.ClientID %>").is(":checked") ? "Organised" :
                 $("#<%= RadioDisorganised.ClientID %>").is(":checked") ? "Disorganised" : "";

             formData.Interactionwithpeers = $("#<%= txtInteractionwithpeers.ClientID %>").val();

             // Stranger anxiety (Present / Absent)
             formData.Strangeranxiety = $("#<%= RadioPresentButton.ClientID %>").is(":checked") ? "Present" :
                 $("#<%= RadioAbsentButton.ClientID %>").is(":checked") ? "Absent" : "";

             // Play with toys (Yes / No / Maybe)
             formData.PlayToys = $("#<%= RadioYesButton.ClientID %>").is(":checked") ? "Yes" :
                                 $("#<%= RadioNoButton.ClientID %>").is(":checked") ? "No" :
                                 $("#<%= RadioMaybeButton.ClientID %>").is(":checked") ? "Maybe" : "";
            
             formData.Preferenceoftoys = $("#<%= txtPreferenceoftoys.ClientID %>").val();
             formData.CommentsPB = $("#<%= txtCommentsPB.ClientID %>").val();

             showConfirmPopup(formData, function () {
                 PostToHandler(formData, reloadAfterSave, "Play Behaviour");
             });
         }
         function SaveTab12(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 12;

             // Brushing (Dependent / Assisted / Independent)
             formData.Brushing = $("#<%= RadioDependent.ClientID %>").is(":checked") ? "Dependent" : "";
             formData.Brushing_1 = $("#<%= RadioAssisted.ClientID %>").is(":checked") ? "Assisted" : "";
             formData.Brushing_2 = $("#<%= RadioIndependent.ClientID %>").is(":checked") ? "Independent" : "";
             formData.CommentsBrushing = $("#<%= txtCommentsBrushing.ClientID %>").val();

             // Bathing
             formData.Bathing = $("#<%= DependentRadio.ClientID %>").is(":checked") ? "Dependent" : "";
             formData.Bathing_1 = $("#<%= AssistedRadio.ClientID %>").is(":checked") ? "Assisted" : "";
             formData.Bathing_2 = $("#<%= IndependentRadio.ClientID %>").is(":checked") ? "Independent" : "";
             formData.CommentsBathing = $("#<%= txtCommentsBathing.ClientID %>").val();

             // Toileting
             formData.Toileting = $("#<%= RadioDependentButton.ClientID %>").is(":checked") ? "Dependent" : "";
             formData.Toileting_1 = $("#<%= RadioAssistedButton.ClientID %>").is(":checked") ? "Assisted" : "";
             formData.Toileting_2 = $("#<%= RadioIndependentButton.ClientID %>").is(":checked") ? "Independent" : "";
             formData.CommentsToileting = $("#<%= txtCommentsToileting.ClientID %>").val();

             // Dressing
             formData.Dressing = $("#<%= RadioButtonDependent.ClientID %>").is(":checked") ? "Dependent" : "";
             formData.Dressing_1 = $("#<%= RadioButtonAssisted.ClientID %>").is(":checked") ? "Assisted" : "";
             formData.Dressing_2 = $("#<%= RadioButtonIndependent.ClientID %>").is(":checked") ? "Independent" : "";
             formData.CommentsDressing = $("#<%= txtCommentsDressing.ClientID %>").val();
           
             // Eating
             formData.Eating   = $("#<%= RadioBtnDependent.ClientID %>").is(":checked") ? "Dependent" : "";
             formData.Eating_1 = $("#<%= RadioBtnAssisted.ClientID %>").is(":checked") ? "Assisted" : "";
             formData.Eating_2 = $("#<%= RadioBtnIndependent.ClientID %>").is(":checked") ? "Independent" : "";
             formData.CommentsEating = $("#<%= txtCommentsEating.ClientID %>").val();
           
             // Ambulation
             formData.Ambulation   = $("#<%= RdoDependent.ClientID %>").is(":checked") ? "Dependent" : "";
             formData.Ambulation_1 = $("#<%= RdoAssisted.ClientID %>").is(":checked") ? "Assisted" : "";
             formData.Ambulation_2 = $("#<%= RdoIndependent.ClientID %>").is(":checked") ? "Independent" : "";
             formData.CommentsAmbulation = $("#<%= txtCommentsAmbulation.ClientID %>").val();
           
             // Transfers
             formData.Transfers   = $("#<%= RdobtnDependent.ClientID %>").is(":checked") ? "Dependent" : "";
             formData.Transfers_1 = $("#<%= RdobtnAssisted.ClientID %>").is(":checked") ? "Assisted" : "";
             formData.Transfers_2 = $("#<%= RdobtnIndependent.ClientID %>").is(":checked") ? "Independent" : "";
             formData.CommentsTransfers = $("#<%= txtCommentsTransfers.ClientID %>").val();

             showConfirmPopup(formData, function () {
                 PostToHandler(formData, reloadAfterSave, "ADL's");
             });
         }
         function SaveTab13(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 13;

             formData.AddComments = $("#<%= txtAddComments.ClientID %>").val();

             showConfirmPopup(formData, function () {
                 PostToHandler(formData, reloadAfterSave, "Observations");
             });
         }
         function SaveTab14(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
             formData.TabNo = 14;

             formData.AddEvalRec = $("#<%= txtAddEvalRec.ClientID %>").val();

             showConfirmPopup(formData, function () {
                 PostToHandler(formData, reloadAfterSave, "Evaluation Recommended");
             });
         }
         function remove_this_option(ctl) {

             var row = $(ctl).closest('.cloneThisRow');

             // get DB ID
             var preConsultID = row.find("input[id*='txtPreConsultID']").val();

             // store deleted ID (only if already saved row)
             if (preConsultID && preConsultID !== "0") {

                 var hdn = $('#<%= hdnDeletedTimelineIds.ClientID %>');
                 var existing = hdn.val();

                 if (existing.indexOf(preConsultID) === -1) {
                     hdn.val(existing ? existing + "," + preConsultID : preConsultID);
                 }
             }

             // hide row (UI)
             row.addClass('hide');

             // clear values so it won't save again
             row.find('input[type="text"], textarea').val('');
             row.find("input[id*='txtPreConsultID']").val("0");

             AddRemoveButton(ctl);
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
                 url: "<%= ResolveUrl("~/Handler/PreconstRpt_2021.ashx") %>",
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
          function updateInternetStatus(statusText, speedText) {
              $("#modalInternetStatus").html(
                  "<div>" +
                  "<span><b>Internet:</b> " + statusText + "</span>" +
                  "<span style='margin-left:20px;'><b>Speed:</b> " + speedText + "</span>" +
                  "</div>" +
                  "<div id='logStatusMsg' style='margin-top:6px; font-size:12px;'></div>"
              );
          }
      </script>

    <%-- <script type="text/javascript">
    
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
                 preTabId = 'TabPanel13_tab';
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
     </script>--%>
         <script type="text/javascript">
             function Changetab(ctl, tabp) {
                 console.log("save", ctl, tabp);
             }
         </script>

    <%-- <script type="text/javascript">
    function goToNextTab() {
  // Show the loader
  document.getElementById("loader").style.display = "block";

  var tabContainer = $find("<%= tb_Contents.ClientID %>");
  var currentTabIndex = tabContainer.get_activeTabIndex();
  var tabCount = tabContainer.get_tabs().length;

  // Calculate the index of the next tab
  var nextTabIndex = (currentTabIndex + 1) % tabCount;

  // Set the next tab as the active tab
  isTabChangeAllowed = false; // Prevent direct tab change

  // Call the server-side click event of the button to handle tab change
  __doPostBack("<%= Button1.UniqueID %>", nextTabIndex.toString());
        }
    </script>--%>

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
         }
         else {
             // Execute code for non-mobile devices
             // Use your existing code or another approach suitable for non-mobile devices
             console.log('Non-mobile resolution detected.');
             if (sessionStorage.getItem('pageOpened')) {
                 // Display a warning message
                 alert('This page is already open in another tab.');
                 // Redirect or take appropriate action
                 window.location.href = '/SessionRpt/PreConsultView.aspx'; // Redirect to another page
             } else {
                 // Set a flag in sessionStorage indicating that the page is open
                 sessionStorage.setItem('pageOpened', 'true');
                 // Add an event listener to handle tab close events
                 window.addEventListener('beforeunload', function () {
                     // Clear the flag when the tab is closed
                     sessionStorage.removeItem('pageOpened');
                 });
             }
         }
     </script>

   <%-- <script>
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

            if (document.cookie.includes('pageOpened=true')) {
                // Display a warning message
                alert('This page is already open in another tab.');
                // Redirect or take appropriate action
                window.location.href = '/SessionRpt/PreConsultView.aspx';  // Redirect to another page
            } else {
                // Set a cookie indicating that the page is open
                document.cookie = 'pageOpened=true; path=/'; // Set the cookie path appropriately
                // Add an event listener to handle tab close events
                window.addEventListener('beforeunload', function () {
                    // Clear the cookie when the tab is closed
                    document.cookie = 'pageOpened=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/'; // Clear the cookie
                });
            }
            // Use your existing logic or alternative approaches here
        }


    </script>--%>

</asp:Content>
