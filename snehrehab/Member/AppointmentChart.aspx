<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="AppointmentChart.aspx.cs" Inherits="snehrehab.Member.AppointmentChart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/fullcalender/fullcalendar.min.css" rel="stylesheet" type="text/css" />
    <link href="/fullcalender/fullcalendar.print.min.css" rel="stylesheet" type="text/css" media='print' />
    <link href="/fullcalender/scheduler.min.css" rel="stylesheet" type="text/css" />
    <script src="/fullcalender/moment.min.js" type="text/javascript"></script>
    <script src="/fullcalender/fullcalendar.min.js" type="text/javascript"></script>
    <script src="/fullcalender/scheduler.min.js" type="text/javascript"></script>
     
    <script src="/js/jspdf.min.js" type="text/javascript"></script>
    <script src="/js/html2canvas.min.js" type="text/javascript"></script> 
    <script type="text/javascript">
        $('<img src="/images/load/new.gif" />');
         var cal_resources = <%= Newtonsoft.Json.JsonConvert.SerializeObject(cal_resources)%>;
         var cal_events = <%= Newtonsoft.Json.JsonConvert.SerializeObject(cal_events)%>;
        $(function () {
            $('#calendar').fullCalendar({
                defaultView: 'agendaDay',
                defaultDate: '2018-04-07',
                editable: false,
                selectable: true,
                eventLimit: true, // allow "more" link when too many events
                contentHeight:"auto",
                contentHeight:"auto",
                handleWindowResize:true,
                minTime : "<%=start_time %>",
                maxTime: "<%=end_time %>",
                slotDuration: '00:15:00',
                slotLabelFormat: 'hh:mm A', 
                timeFormat: 'hh:mm A',
                displayEventTime : false,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'agendaDay,agendaTwoDay,agendaWeek,month'
                },
                views: {
                    agendaTwoDay: {
                        type: 'agenda',
                        duration: { days: 2 },
                        // views that are more than a day will NOT do this behavior by default
                        // so, we need to explicitly enable it
                        groupByResource: true
                        //// uncomment this line to group by day FIRST with resources underneath
                        //groupByDateAndResource: true
                    }
                },
                //// uncomment this line to hide the all-day slot
                allDaySlot: false,
                resources: cal_resources,
                events: cal_events,
                select: function (start, end, jsEvent, view, resource) {
                    //console.log('select', start.format("YYYY-MM-DD HH:mm:SS"), end.format("YYYY-MM-DD HH:mm:SS"), resource ? resource.id : '(no resource)');
                },
                dayClick: function (date, jsEvent, view, resource) {
                    //console.log('dayClick', date.format("YYYY-MM-DD HH:mm:SS"), resource ? resource.id : '(no resource)');
                },
                eventClick: function (event, jsEvent, view) {
                    var ctl = $(this);
                    ShowAppointment(event.id,event.resourceId, event.table, ctl);
                },
                eventRender: function (event, element) {
                    element.find('.fc-title').html(event.title);
                },
                eventAfterAllRender : function(event, element, view) {
                    $('.fc-widget-header thead .fc-widget-header').html('TIME');
                    $('.fc-widget-header thead .fc-widget-header').css({'text-align':'center'});
                }
            }); 
        });
    </script>
    <script type="text/javascript">
        function ShowAppointment(id,resourceId, table, ctl) {
            $.ajax({
                url: '/Member/AptDetail.ashx?table=' + table + '&id=' + id + '&resourceid=' + resourceId, contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $(ctl).find('.fc-bg').html('<div class=\"event_loader\"></div>');
                },
                success: function (result) {
                    if (result.status) {
                        $('#apt_modal .modal-content').html(result.data);
                        $('#apt_modal').unbind('hidden.bs.modal');
                        $('#apt_modal').modal().show();
                    } else {
                        alert(result.msg);
                    }
                },
                complete: function () {
                    $(ctl).find('.fc-bg .event_loader').remove();
                }
            });
        }
        var new_apt_id = ''; var new_pat_id = '';
        function fwdToRegistration(a, p) {
            $('#apt_modal').modal('hide').on('hidden.bs.modal', function () {
                new_apt_id = a; new_pat_id = p;
                $('#patient_type_modal').modal().show();
            });
        }
        function NextToRegistration() {
            var t = $('#txt_new_patient_type').val();
            if (t == 1) {
                window.location = '/Member/Adult.aspx?record=' + new_pat_id + '&apt=' + new_apt_id + '&return=101';
            } else if (t == 2) {
                window.location = '/Member/Pediatric.aspx?record=' + new_pat_id + '&apt=' + new_apt_id + '&return=101';
            }
        }
    </script>
    <script type="text/javascript">
        function WaitingConfirm(id) {
            if (confirm('Are you sure to confirm.?')) {
                $.ajax({
                    url: '/Member/WaitingConfirm.ashx?id=' + id, contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        if (result.status) {
                            window.location = '/Member/AppointmentPay.aspx?return=101&record=' + result.data; 
                        } else {
                            alert(result.msg);
                        }
                    }
                });
            }
        }
        function WaitingCancel(id) {
            if (confirm('Are you sure to cancel.?')) {
                $.ajax({
                    url: '/Member/WaitingCancel.ashx?id=' + id, contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        if (result.status) {
                            window.location = '/Member/AppointmentChart.aspx';
                        } else {
                            alert(result.msg);
                        }
                    }
                });
            }
        }
    </script>    
    <script type="text/javascript">
        function ExportPDF(ctl) {
            $(ctl).val('Wait...');
            $(ctl).attr('disabled', 'disabled')
            $('#calendar td.fc-time, #calendar th.fc-axis').each(function(){
                $(this).css({'height':$(this).height()+'px'});
            }); 
            $('#lbl_curr_date').html('Appointments Date : - '+$('#<%=txtSearchDate.ClientID%>').val());
            html2canvas($('#calender_canvas').get(0), {
            letterRendering: 1, 
            allowTaint : true,
            removeContainer:false,
            }).then(function (canvas) { 
                $('#lbl_curr_date').html('');
                var imgData = canvas.toDataURL("image/jpeg", 1); 
                //console.log(imgData);
                //window.open(imgData); 
                var doc = new jsPDF();
                doc.addImage(imgData, 'JPEG', 15, 40, 180, 160);
                $(ctl).val('Export');
                $(ctl).removeAttr('disabled')
                download(doc.output(), "Appointment Chart.pdf", "text/pdf");
            }); 
        }
        function download(strData, strFileName, strMimeType) {
            var D = document, A = arguments, a = D.createElement("a"), d = A[0], n = A[1], t = A[2] || "text/plain";
            //build download link:
            a.href = "data:" + strMimeType + "," + escape(strData);
            if (window.MSBlobBuilder) {
                var bb = new MSBlobBuilder();
                bb.append(strData);
                return navigator.msSaveBlob(bb, strFileName);
            } /* end if(window.MSBlobBuilder) */
            if ('download' in a) {
                a.setAttribute("download", n);
                a.innerHTML = "downloading...";
                D.body.appendChild(a);
                setTimeout(function () {
                    var e = D.createEvent("MouseEvents");
                    e.initMouseEvent("click", true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
                    a.dispatchEvent(e);
                    D.body.removeChild(a);
                }, 66);
                return true;
            } /* end if('download' in a) */
            //do iframe dataURL download:
            var f = D.createElement("iframe");
            D.body.appendChild(f);
            f.src = "data:" + (A[2] ? A[2] : "application/octet-stream") + (window.btoa ? ";base64" : "") + "," + (window.btoa ? window.btoa : escape)(strData);
            setTimeout(function () {
                D.body.removeChild(f);
            }, 333);
            return true;
        }  /* end download() */ 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="apt_modal" class="modal fade" role="dialog" <%--data-backdrop="static" data-keyboard="false"--%>>
        <div class="modal-dialog modal-sm" role="document" style="">
            <div class="modal-content">
            </div>
        </div>
    </div>   
    <div id="patient_type_modal" class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" role="document" style="">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="opacity: 0.8;"><span aria-hidden="true">×</span></button>
                    <h5 class="modal-title" style="margin:0px;">Patient Type</h5>
                </div>
                <div class="modal-body">
                     <div class="form-horizontal">
                        <div class="formRow">
                            <label class="span1" style="width: 100px;padding-top: 5px;">Select Type</label>
                            <select id="txt_new_patient_type" class="span2">
                                <option value="1">Adult Registration</option>
                                <option value="2">Pediatric Registration</option>
                            </select> 
                        </div>
                     </div>
                </div>
                <div class="modal-footer">
                    <a href="javascript:;" class="btn btn-sm btn-primary" onclick="NextToRegistration();"> Next </a>
                    &nbsp;
                    <button class="btn btn-sm btn-default" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="txtSearchDate" runat="server" />
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Appointment Chart :
            </div>
             <div class="pull-right">
                <asp:Literal ID="lblAddNew" runat="server"></asp:Literal> 
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="txtStatus" runat="server" CssClass="input-medium chzn-select span2">
                    <asp:ListItem Value="0">Pending</asp:ListItem>
                    <asp:ListItem Value="1">Completed</asp:ListItem>
                    <asp:ListItem Value="2">Absent</asp:ListItem>
                    <asp:ListItem Value="10">Cancelled</asp:ListItem>
                    <asp:ListItem Value="-1" Selected="True">All Record</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="ddl_Session" runat="server" CssClass="chzn-select input-medium span3"> 
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="txtTherapist" runat="server" CssClass="chzn-select input-medium span3"> 
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name" CssClass="span2"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="On Date" Width="100px"></asp:TextBox>
                </div> 
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click" Text="Search" />
                    &nbsp;
                    <input type="button" class="btn btn-danger" onclick="ExportPDF(this);return false;" value="Export" />                    
                </div>
            </div>
            <div class="clearfix"></div> 
            <div id="calender_canvas">
                <style type="text/css">
                .fc-toolbar, .fc-license-message{display:none !important;}
                .fc-widget-header .fc-resource-cell{text-transform:uppercase;}
                .appointment-complete{background-color: #3C8600;border: 1px solid #3b8004;}
                 .appointment-schedule{background-color: navy; border:1px solid navy;}
                .appointment-absent{background-color: #ff0024;border: 1px solid #c36501;}
                .appointment-cancel{background-color: #ff8400;border: 1px solid #c36501;}
                .appointment-pre{background-color: #9C27B0;border: 1px solid #7c218c;}
                .appointment-wait{background-color: #9E9E9E;border: 1px solid #868585;}
                span.stat_pending{color: #FFF;padding: 4px 5px;font-style: italic;font-weight: bold;background-color: #3a87ad;}
                span.stat_Schedule{color:#FFF;padding:4px 5px;font-style:italic;font-weight:bold;background-color:navy;}
                span.stat_complete{color: #FFF;padding: 4px 5px;font-style: italic;font-weight: bold;background-color: #54bc00;}
                span.stat_absent{color: #FFF;padding: 4px 5px;font-style: italic;font-weight: bold;background-color: #ff0024;}
                span.stat_cancel{color: #FFF;padding: 4px 5px;font-style: italic;font-weight: bold;background-color: #ff8400;}
                span.stat_prebooking{color: #FFF;padding: 4px 5px;font-style: italic;font-weight: bold;background-color: #9C27B0;}
                span.stat_wait{color: #FFF;padding: 4px 5px;font-style: italic;font-weight: bold;background-color: #9E9E9E;}
                .event_loader{width: 100%;height: 100%;background: url(/images/load/new.gif) no-repeat center center;}
                .fc-event{font-size: 1.1em;}
                .fc-content{padding:4px;}
                label.apt_legend{border: 1px solid #61a4f0;padding: 5px 5px 3px 5px;margin-left: 5px;margin-right: 5px;display: inline-block;}
                label.apt_legend span{display: inline-block;margin-right: 8px;padding: 5px 10px;}
                label.apt_legend label{display: inline-block;font-size: 12px;margin-bottom: 0px;}
                </style>
                <center style="text-align: right;margin-top: 5px;">
                    <div id="lbl_curr_date" style="text-align: center;font-size: 20px;padding-bottom: 20px;"></div>
                    <label class="apt_legend"><span class="stat_Schedule"></span><label>Schedule</label></label>
                    <label class="apt_legend"><span class="stat_pending"></span><label>Confirm</label></label> 
                    <label class="apt_legend"><span class="stat_complete"></span><label>Complete</label></label> 
                    <label class="apt_legend"><span class="stat_cancel"></span><label>Cancel</label></label> 
                    <label class="apt_legend"><span class="stat_absent"></span><label>Absent</label></label> 
                    <label class="apt_legend"><span class="stat_prebooking"></span><label>Pre Booking</label></label> 
                    <label class="apt_legend"><span class="stat_wait"></span><label>Waiting</label></label> 
                </center> 
                <div id='calendar'></div>
                <div style="clear:both;"></div>
                <hr style="border: none;margin: 0px;" />
            </div>
        </div>
    </div> 
</asp:Content>
