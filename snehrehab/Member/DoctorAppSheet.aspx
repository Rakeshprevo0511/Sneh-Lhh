<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="DoctorAppSheet.aspx.cs" Inherits="snehrehab.Member.DoctorAppSheet" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.report-gv-top-align th{vertical-align:top !important;}
.report-gv-top-align td{vertical-align:top !important;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Appointment Sheet :</div>
            <div class="pull-right">
                <a href="/Reports/" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div style="float:left;">
                <div class="span2" style="width: 100px;margin-left: 15px;margin-right:15px;">
                <asp:TextBox ID="txtDate" runat="server" CssClass="my-datepicker span2" placeholder="Select Date" Width="100px"></asp:TextBox>
                </div>
                </div>
                <div style="float:left;margin-top: 4px;">
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click" Visible="false">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <div class="jquery_page_msg"></div>
            <div style="white-space: nowrap;overflow-x:auto;">
            <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered report-gv-top-align" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="ReportGV_PageIndexChanging"
                PageSize="51" AllowPaging="false" OnDataBound="ReportGV_DataBound">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="TIME"><ItemTemplate><%#FORMATTIME(Eval("TimeHourNew").ToString())%></ItemTemplate><HeaderStyle Width="60px" /></asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </div>
    </div>
    
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title" style="margin: 0px;">
                        Change Appointment</h5>
                </div>
                <div class="modal-body">
                     <div class="form-horizontal">
                        <div id="txt_loading_msg" class="alert alert-warning"><strong>Info: </strong> Loading Data, Please wait...</div>
                        <div class="jquery_popup_msg"></div>
                        <div class="formRow">
                            <div class="span5" style="margin: 0px;">
                                <span class="span2"><b>New Doctor:</b></span>
                                <div class="control-group">
                                    <select id="txt_new_doctor" class="chzn-select span3"></select>
                               </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="formRow">
                            <div class="span5" style="margin: 0px;">
                                <span class="span2"><b>Remark:</b></span>
                                <div class="control-group">
                                    <textarea id="txt_new_remark" class="span3"></textarea>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="formRow">
                            <div class="span5" style="margin: 0px;">
                                <span class="span2"><b>Payment Name:</b></span>
                                <div class="control-group">
                                    <input id="txt_change_name" type="text" class="span3" readonly="readonly" />
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="formRow">
                            <div class="span5" style="margin: 0px;">
                                <span class="span2"><b>Session Name:</b></span>
                                <div class="control-group">
                                    <input id="txt_session_name" type="text" class="span3" readonly="readonly" />
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                     </div>
                </div>
                <div class="modal-footer">
                    <a href="javascript:;" class="btn btn-sm btn-default" data-dismiss="modal">Cancel</a>
                    &nbsp;
                    <input type="button" class="btn btn-sm btn-danger" value="Submit Request" onclick="SetAppointment();return false;" />
                    <input type="hidden" id="txt_appointment_id" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function LoadAppointment(a) {
            $('#txt_loading_msg').show(); $("#txt_change_name").val('');$('#txt_new_remark').val('');  $("#txt_appointment_id").val(''); $("#txt_session_name").val(''); $('#txt_new_doctor option').remove(); Dropdown_Chzn_select('txt_new_doctor');
            $('#myModal div.modal-footer').hide('fast'); $('#myModal div.modal-footer').css({ "display": "none" });
            $.ajax({
                type: "POST", url: "/Snehrehab.asmx/ChangeAppointmentDoctor", data: "{'d':'" + a + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function(response) {
                    $("#txt_appointment_id").val(a);
                    $("#txt_change_name").val(response.d[0][0]);
                    $("#txt_session_name").val(response.d[0][1]);
                    $('#txt_new_doctor').append('<option value="-1">NO TRANSFER</option>');
                    for (var i = 0; i < response.d[1].length; i++) {
                        $('#txt_new_doctor').append('<option value="' + response.d[1][i][0] + '">' + response.d[1][i][1] + '</option>');
                    }
                    Dropdown_Chzn_select('txt_new_doctor');
                },
                error: function(msg) { },
                complete: function() { Dropdown_Chzn_select('txt_new_doctor'); $('#txt_loading_msg').hide(); $('#myModal div.modal-footer').slideDown('slow'); }
            });
        }
        function SetAppointment() {
            var a = $("#txt_appointment_id").val();
            if (a.length > 0) {
                var n = $('#txt_new_doctor').val(); if (isNaN(n)) { n = 0; }
                var r = $('#txt_new_remark').val().trim();
                if (r.length <= 0) {
                    $('#txt_new_remark').focus(); return false;
                } else {
                    $('#myModal div.modal-footer').slideUp('fast');
                    $.ajax({
                        type: "POST", url: "/Snehrehab.asmx/ChangeAppointmentRequest", data: "{'d':'" + a + "', 'n':" + n + ", 'r':'" + r + "'}",
                        contentType: "application/json; charset=utf-8", dataType: "json",
                        success: function(response) {
                            if (response.d > 0) {
                                $('#myModal div.modal-header .close').trigger('click');
                                $("html, body").animate({ scrollTop: 0 }, 500);
                                $('.jquery_page_msg').html('<div class="alert alert-success alert-dismissible"><button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span></button><strong>Success !</strong> Appointment change request submitted successfully.</div>');
                            }
                            else if (response.d == -10) {
                                $('#myModal div.modal-header .close').trigger('click');
                                $("html, body").animate({ scrollTop: 0 }, 500);
                                $('.jquery_page_msg').html('<div class="alert alert-info alert-dismissible"><button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span></button><strong>Info !</strong> Your request is already submitted.</div>');
                            }
                            else {
                                $('.jquery_popup_msg').html('<div class="alert alert-danger alert-dismissible"><button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span></button><strong>Error !</strong> Unable to process your request, please try again.</div>');
                                $('#myModal div.modal-footer').slideDown('slow');
                            }
                        },
                        error: function(msg) { },
                        complete: function() { }
                    });
                }
            }
        }
    </script>
</asp:Content>
