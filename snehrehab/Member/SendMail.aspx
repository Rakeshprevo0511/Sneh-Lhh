<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="SendMail.aspx.cs" Inherits="snehrehab.Member.SendMail" %>
<%@ MasterType VirtualPath="~/Member/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .add_new
        {
            margin-left: 11px;
            background: #4e8de0;
            color: #fff;
            padding: 5px 10px;
            font-size: 11px;
            height: 31px;
        }

        a:hover
        {
            color: #ffffff!important;
            text-decoration: underline;
        }
        input[type="radio"] {
            display: block;
            margin-right: 10px;
        }
        .d-flex
        {
               display: flex;
        }
        .btn.btn-danger
        {
            margin-left: 10px;
            line-height: 22px;
            height: 30px;
        }
        .ajax-loader {
          visibility: hidden;
            background-color: rgb(255 255 255 / 18%);
            position: absolute;
            z-index: +100 !important;
            left: 49%;
            top: 33%;
        }

        .ajax-loader img {
          width: 35%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <asp:UpdatePanel ID="new" runat="server">
            <ContentTemplate>
                <asp:PlaceHolder ID="msg" runat="server"></asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="grid-title">
            <div class="pull-left">
                Mail Receipt :
            </div>
        </div>
        <div class="grid-content">
            <div class="ajax-loader">
              <img src='/Files/circle-transp.gif' class="img-responsive" />
            </div>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="Hidreceivertype" runat="server" />
                    <div class="formRow">
                        <div class="span8" style="display: flex;">
                            <asp:RadioButton ID="rdpat" runat="server" GroupName="Group1" AutoPostBack="true" OnCheckedChanged="rdpat_CheckedChanged" />
                            <label class="control-label">
                                Patient MailID :</label>
                            <div class="control-group">
                                <%--<asp:DropDownList ID="txtpatientmail" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtpatientmail_SelectedIndexChanged">
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txtpatientmail" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                            </div>
                            <input id="btnadd" type="button" value="Update" onclick="LoadDetail(1)" class="btn btn-primary add_new"
                                    runat="server" />

                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="formRow">
                        <div class="span8" style="display: flex;">
                            <asp:RadioButton ID="rdfat" runat="server" GroupName="Group1" AutoPostBack="true" OnCheckedChanged="rdfat_CheckedChanged" />
                            <label class="control-label">
                                Father MailID:</label>
                            <div class="control-group">
                                <%--<asp:DropDownList ID="txtfathermail" runat="server" CssClass="chzn-select span4" AutoPostBack="true" OnSelectedIndexChanged="txtfathermail_SelectedIndexChanged">
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txtfathermail" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                            </div>
                            <input id="Button1" type="button" value="Update" onclick="LoadDetail(2)" class="btn btn-primary add_new"
                                    runat="server" />
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="formRow">
                        <div class="span8" style="display: flex;">
                            <asp:RadioButton ID="rdmot" runat="server" GroupName="Group1" AutoPostBack="true" OnCheckedChanged="rdmot_CheckedChanged" />
                            <label class="control-label">
                                Mother MailID:</label>
                            <div class="control-group">
                                <%--<asp:DropDownList ID="txtmothermail" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtmothermail_SelectedIndexChanged">
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txtmothermail" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                            </div>
                            <input id="Button2" type="button" value="Update" onclick="LoadDetail(3)" class="btn btn-primary add_new"
                                    runat="server" />
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="formRow">
                        <div class="span8" style="display: flex;">
                            <asp:RadioButton ID="rdref" runat="server" GroupName="Group1" AutoPostBack="true" OnCheckedChanged="rdref_CheckedChanged" />
                            <label class="control-label">
                                Reference MailID:</label>
                            <div class="control-group">
                                <%--<asp:DropDownList ID="txtrefmail" runat="server" CssClass="chzn-select span4" OnSelectedIndexChanged="txtrefmail_SelectedIndexChanged">
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txtrefmail" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                            </div>
                            <input id="btn3" type="button" value="Update" onclick="LoadDetail(4)" class="btn btn-primary add_new"
                                    runat="server" />
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>                    
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="updmail" runat="server">
                <ContentTemplate>
                    <div id="mailsend" runat="server" visible="false">
                        <div class="formRow" style="    margin-top: 15px;">
                            <div class="span8" style="display: flex;padding-left: 20px;">
                                <asp:RadioButton ID="RadioButton1" runat="server" GroupName="Group1" AutoPostBack="false" Visible="false"/>
                                <label class="control-label" style="font-size:13px;">
                                    MailID:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtmail" runat="server" CssClass="span4"></asp:TextBox>
                                </div>
                                <%--<asp:LinkButton ID="send" runat="server" Text="Send" CssClass="btn btn-danger"
                                OnClick="send_Click"></asp:LinkButton>--%>
                                <input id="btnsendmail" type="button" value="Send" class="btn btn-danger" onclick="MailReceipt();" />
                                <%--<input id="btnsendmail" type="button" value="Send" class="btn btn-danger" onclick="doFunction();" />--%>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <hr />
            <%--<div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        &nbsp;</label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-danger"
                            OnClientClick="DisableOnSubmit(this);" OnClick="btnSubmit_Click"></asp:LinkButton>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>--%>
        </div>
    </div>
    <asp:HiddenField id="hidValue" runat="server"/>
    <asp:HiddenField id="hidtype" runat="server"/>
    <asp:HiddenField ID="hidrectype" runat="server" />
    <asp:HiddenField ID="hidreceiptno" runat="server" />
    <asp:HiddenField ID="Hiduniqueid" runat="server" />
    <asp:HiddenField ID="Hidfiscaldate" runat="server" />
    <asp:HiddenField ID="HidSiType" runat="server" />
    <asp:HiddenField ID="HidAppointID" runat="server" />
    <asp:HiddenField ID="HidDailType" runat="server" />
    <asp:HiddenField ID="HidNdtType" runat="server" />
     <asp:HiddenField ID="HidNdt_newType" runat="server" />
    <asp:HiddenField ID="HidRevType" runat="server" />
    <asp:HiddenField ID="HidBotType" runat="server" />
    <asp:HiddenField ID="HidPreType" runat="server" />
    <asp:HiddenField ID="HidEipType" runat="server" />
    <asp:HiddenField ID="HidReciver" runat="server" />
     <asp:HiddenField ID="Hidsirpt2022" runat="server"/>
    
    <div class="modal fade" id="mail_modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Mail</h4>
                </div>
                <div class="modal-body">
                    <div class="msg-container">
                    </div>                   
                    <div class="formRow">
                        <div class="span5" style="margin: 0px;">
                            <span class="span1"><b>Email:</b></span>
                            <div class="control-group">
                                <div class="span3">
                                    <asp:TextBox ID="txtupdatemail" runat="server" CssClass="span3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">                    
                    <div style="float: right;">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        &nbsp;
                        <button type="button" class="btn btn-primary" onclick="UpdateMail(this);">Update</button>                        
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="urlid" runat="server" />
    </div>
    <script type="text/javascript">
        function LoadDetail(id) {
            $('#<%=hidtype.ClientID %>').val(id);
            var userid = id;
            var PatientID = $('#<%=hidValue.ClientID %>').val();
            $.get('/Member/MailIDGet.ashx?id=' + PatientID + '&user=' + userid, function (result) {
                if (result.status) {
                    mailid = result.data.MailID;
                    $('#<%=txtupdatemail.ClientID%>').val(mailid);
                    $('#mail_modal').modal('show').off('hidden.bs.modal');
                }
                else {
                    alert(result.msg);
                }

            });

        }

        function UpdateMail(ctl) {

            var mail = $('#<%=txtupdatemail.ClientID %>').val();
            var data = {};
            if (mail.length > 0) {
                data.MailId = $('#<%=txtupdatemail.ClientID %>').val();
                data.PatientID = $('#<%=hidValue.ClientID %>').val();
                data.Type = $('#<%=hidtype.ClientID %>').val();
                $.ajax({
                    type: "POST", url: '/Member/UpdateMail.ashx',
                    contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                    success: function (result) {
                        if (result.status) {
                            $('#mail_modal').modal('hide').on('hidden.bs.modal', function () {
                                window.location.reload();
                                alert('Mail updated successfully.');

                            });
                        } else {
                            AlertMessage('#mail_modal .msg-container', result.msg, 2);
                        }
                    },
                    error: function (msg) {
                        AlertMessage('#mail_modal .msg-container', msg.responseText, 2);
                    },
                    complete: function () { $(ctl).removeAttr('disabled'); }
                });
            }
            else {
                AlertMessage('#mail_modal .msg-container', "Please enter mail", 2);
            }

        }

        function MailReceipt() {
            var mail = $('#<%=txtmail.ClientID %>').val();
            var data = {};
            data.MailId = $('#<%=txtmail.ClientID %>').val();
            data.Receivertype = $('#<%=Hidreceivertype.ClientID%>').val();
            
            if (mail.length > 0) {
                var sitype=$('#<%=HidSiType.ClientID %>').val();
                var siapointid = $('#<%=HidAppointID.ClientID%>').val();
                var daitype = $('#<%=HidDailType.ClientID %>').val();
                var reciptype = $('#<%=hidrectype.ClientID %>').val();
                var ndttype = $('#<%=HidNdtType.ClientID %>').val();
                var ndttype_new = $('#<%=HidNdt_newType.ClientID %>').val();
                var revtype = $('#<%=HidRevType.ClientID %>').val();
                var bottype = $('#<%=HidBotType.ClientID %>').val();
                var presctype=$('#<%=HidPreType.ClientID %>').val();
                var eiptype = $('#<%=HidEipType.ClientID %>').val();
                var mreciver = $('#<%=HidReciver.ClientID %>').val();
                var typesi2022 = $('#<%=Hidsirpt2022.ClientID %>').val();
                
                if (reciptype.length > 0) {
                    data.ReceiptType = $('#<%=hidrectype.ClientID %>').val();
                    data.ReceiptNo = $('#<%=hidreceiptno.ClientID %>').val();
                    data.UniqueID = $('#<%=Hiduniqueid.ClientID %>').val();
                    data.FiscalDate = $('#<%=Hidfiscaldate.ClientID %>').val();
                    data.PatientID = $('#<%=hidValue.ClientID %>').val();
                    data.Reciever = $('#<%=HidReciver.ClientID %>').val();
                    var sa = $('#<%=HidReciver.ClientID %>').val()
                    alert(sa);
                    $('#loading-image').show();
                    $.ajax({
                        type: "POST", url: '/Handler/MailReceipt_PDFNew.ashx',
                        beforeSend: function () {
                            $('.ajax-loader').css("visibility", "visible");
                        },
                        contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                        success: function (result) {
                            if (result.status) {
                                window.location.reload();
                                alert('Mail Send Successfully');
                            } else {
                                alert('Unable to process...');
                            }
                        },
                        error: function (msg) {
                            alert('Unable to process...');
                        },
                        complete: function () { $('.ajax-loader').css("visibility", "hidden"); }
                    });
                }

                else if (daitype.length > 0 && daitype == 'dai') {
                    data.SiAppointmentID = $('#<%=HidAppointID.ClientID%>').val();
                    $('#loading-image').show();
                    $.ajax({
                        type: "POST", url: '/Handler/Daily_Report_MailPDF.ashx',
                        beforeSend: function () {
                            $('.ajax-loader').css("visibility", "visible");
                        },
                        contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                        success: function (result) {
                            if (result.status) {
                                window.location.reload();
                                alert('Mail Send Successfully');
                            } else {
                                alert('Unable to process...');
                            }
                        },
                        error: function (msg) {
                            alert('Unable to process...');
                        },
                        complete: function () { $('.ajax-loader').css("visibility", "hidden"); }
                    });
                }
                else if (ndttype.length > 0 && ndttype == 'ndt') {
                    data.SiAppointmentID = $('#<%=HidAppointID.ClientID%>').val();
                    $('#loading-image').show();
                    $.ajax({
                        type: "POST", url: '/Handler/Ndt_Report_MailPDF.ashx',
                        beforeSend: function () {
                            $('.ajax-loader').css("visibility", "visible");
                        },
                        contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                        success: function (result) {
                            if (result.status) {
                                window.location.reload();
                                alert('Mail Send Successfully');
                            } else {
                                alert('Unable to process...');
                            }
                        },
                        error: function (msg) {
                            alert('Unable to process...');
                        },
                        complete: function () { $('.ajax-loader').css("visibility", "hidden"); }
                    });
                }
                else if (ndttype_new.length > 0 && ndttype_new == 'ndt_new') {
                    data.SiAppointmentID = $('#<%=HidAppointID.ClientID%>').val();
                    $('#loading-image').show();
                    $.ajax({
                        type: "POST", url: '/Handler/Ndt_Report_MailPDF_new.ashx',
                        beforeSend: function () {
                            $('.ajax-loader').css("visibility", "visible");
                        },
                        contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                        success: function (result) {
                            if (result.status) {
                                window.location.reload();
                                alert('Mail Send Successfully');
                            } else {
                                alert('Unable to process...');
                            }
                        },
                        error: function (msg) {
                            alert('Unable to process...');
                        },
                        complete: function () { $('.ajax-loader').css("visibility", "hidden"); }
                    });
                }
                else if (bottype.length > 0 && bottype == 'botox') {
                    data.SiAppointmentID = $('#<%=HidAppointID.ClientID%>').val();
                    $('#loading-image').show();
                    $.ajax({
                        type: "POST", url: '/Handler/Botox_Report_MailPDF.ashx',
                        beforeSend: function () {
                            $('.ajax-loader').css("visibility", "visible");
                        },
                        contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                        success: function (result) {
                            if (result.status) {
                                window.location.reload();
                                alert('Mail Send Successfully');
                            } else {
                                alert('Unable to process...');
                            }
                        },
                        error: function (msg) {
                            alert('Unable to process...');
                        },
                        complete: function () { $('.ajax-loader').css("visibility", "hidden"); }
                    });
                }
                else if (sitype.length > 0 && sitype == 'si') {
                    data.SiAppointmentID = $('#<%=HidAppointID.ClientID%>').val();
                    $('#loading-image').show();
                    $.ajax({
                        type: "POST", url: '/Handler/Si_Report_MailPDF.ashx',
                        beforeSend: function () {
                            $('.ajax-loader').css("visibility", "visible");
                        },
                        contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                        success: function (result) {
                            if (result.status) {
                                window.location.reload();
                                alert('Mail Send Successfully');
                            } else {
                                alert('Unable to process...');
                            }
                        },
                        error: function (msg) {
                            alert('Unable to process...');
                        },
                        complete: function () { $('.ajax-loader').css("visibility", "hidden"); }
                    });
                }
                else if (revtype.length > 0 && revtype == 'rev') {
                    data.SiAppointmentID = $('#<%=HidAppointID.ClientID%>').val();
                    $('#loading-image').show();
                    $.ajax({
                        type: "POST", url: '/Handler/Reval_Report_MailPDF.ashx',
                        beforeSend: function () {
                            $('.ajax-loader').css("visibility", "visible");
                        },
                        contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                        success: function (result) {
                            if (result.status) {
                                window.location.reload();
                                alert('Mail Send Successfully');
                            } else {
                                alert('Unable to process...');
                            }
                        },
                        error: function (msg) {
                            alert('Unable to process...');
                        },
                        complete: function () { $('.ajax-loader').css("visibility", "hidden"); }
                    });
                }
                else if (eiptype.length > 0 && eiptype == 'eip') {
                    data.SiAppointmentID = $('#<%=HidAppointID.ClientID%>').val();
                    $('#loading-image').show();
                    $.ajax({
                        type: "POST", url: '/Handler/EIP_Report_MailPDF.ashx',
                        beforeSend: function () {
                            $('.ajax-loader').css("visibility", "visible");
                        },
                        contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                        success: function (result) {
                            if (result.status) {
                                window.location.reload();
                                alert('Mail Send Successfully');
                            } else {
                                alert('Unable to process...');
                            }
                        },
                        error: function (msg) {
                            alert('Unable to process...');
                        },
                        complete: function () { $('.ajax-loader').css("visibility", "hidden"); }
                    });
                }
                else if (presctype.length > 0 && presctype == 'presc') {
                    data.SiAppointmentID = $('#<%=HidAppointID.ClientID%>').val();
                    $('#loading-image').show();
                    $.ajax({
                        type: "POST", url: '/Handler/Presc_Report_MailPDF.ashx',
                        beforeSend: function () {
                            $('.ajax-loader').css("visibility", "visible");
                        },
                        contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                        success: function (result) {
                            if (result.status) {
                                window.location.reload();
                                alert('Mail Send Successfully');
                            } else {
                                alert('Unable to process...');
                            }
                        },
                        error: function (msg) {
                            alert('Unable to process...');
                        },
                        complete: function () { $('.ajax-loader').css("visibility", "hidden"); }
                    });
                }

                else if (typesi2022.length > 0 && typesi2022 == 'si2022') {
                    {
                        data.SiAppointmentID = $('#<%=HidAppointID.ClientID%>').val();
                        $('#loading-image').show();
                        $.ajax({
                            type: "POST", url: '/Handler/Si2022_mailaPdf.ashx',
                            beforeSend: function () {
                                $('.ajax-loader').css("visibility", "visible");
                            },
                            contentType: "application/json; charset=utf-8", data: JSON.stringify(data),
                            success: function (result) {
                                if (result.status) {
                                    window.location.reload();
                                    alert('Mail Send Successfully');
                                } else {
                                    alert('Unable to process...');
                                }
                            },
                            error: function (msg) {
                                alert('Unable to process...');
                            },
                            complete: function () { $('.ajax-loader').css("visibility", "hidden"); }
                        });
                    }
                }
            }
            else {
                alert('Please enter mailid');
            }
            
        }
    </script>
    <asp:UpdateProgress ID="UpdatePanelProgress" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="loading_message_mask">
            </div>
            <div class="loading_message">
                Please Wait...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
