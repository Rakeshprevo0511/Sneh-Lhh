<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="View_CerDeg_List.aspx.cs" Inherits="snehrehab.Member.View_CerDeg_List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .btn
        {
            padding: 5px 14px;
            line-height: 30px;
        }
    </style>
    <style>
        .grid-pagination span
        {
            position: relative;
            float: left;
            padding: 6px 12px;
            text-decoration: none;
            color: #fff;
            background-color: #337ab7;
            border: 1px solid #337ab7;
            margin-left: -2px;
            z-index: 3;
            cursor: default;
        }
        
        .grid-pagination a
        {
            position: relative;
            float: left;
            padding: 6px 13px;
            text-decoration: none;
            color: #337ab7;
            background-color: #ffffff;
            border: 1px solid #dddddd;
            margin-left: -1px;
            z-index: 3;
            cursor: pointer;
        }
        
        th:nth-child(1) {
          width:6%;
        padding: 5px;
        }
        th:nth-child(2) {
          width:13%;
            padding: 5px;
        }
         td:nth-child(1)
         {
             text-align:center;
        }
        td:nth-child(2)
        {
            text-align:center;
        }
         
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                All List :
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:UpdatePanel ID="dropdwn" runat="server">
                    <ContentTemplate>
                        <div class="pull-left" style="display: inline-block; margin-right: 10px;">
                            <asp:DropDownList ID="txtType" runat="server" CssClass="input-medium chzn-select span2"
                                AutoPostBack="true" OnSelectedIndexChanged="txtType_SelectedIndexChanged">
                                <asp:ListItem Value="0">All</asp:ListItem>
                                <asp:ListItem Value="1">Receiption</asp:ListItem>
                                <asp:ListItem Value="2">Management</asp:ListItem>
                                <asp:ListItem Value="3">Doctor</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="pull-left" style="display: inline-block; margin-right: 5px;">
                            <asp:DropDownList ID="txtid" runat="server" CssClass="chzn-select input-medium span3">
                            </asp:DropDownList>
                        </div>
                        <%--<asp:TextBox ID="txtSearch" runat="server" placeholder="Name"></asp:TextBox>--%>
                        <div class="pull-left" style="display: inline-block; margin-right: 4px;">
                            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" OnClick="btnSearch__Click">Search</asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <asp:UpdatePanel ID="rpt" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="txtMain" runat="server" OnRowDataBound="txtMain_OnRowDataBound"
                        AutoGenerateColumns="false" PageSize="5" AllowPaging="true" OnPageIndexChanging="txtMain_PageIndexChanging" Width="100%">
                        <PagerStyle ForeColor="Black" CssClass="grid-pagination" HorizontalAlign="Center" />
                        <PagerSettings FirstPageText="<<" PageButtonCount='10' LastPageText=">>" Mode="NumericFirstLast" />
                        <Columns>
                            <asp:TemplateField HeaderText="SR.NO.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                    <asp:HiddenField ID="hidrecid" runat="server" Value='<%# Eval("ReceiptionID") %>' />
                                    <asp:HiddenField ID="hiddocid" runat="server" Value='<%# Eval("DoctorID") %>' />
                                    <asp:HiddenField ID="hidmanid" runat="server" Value='<%# Eval("ManagerID") %>' />
                                    <asp:HiddenField ID="hidnewid" runat="server" Value='<%# Eval("Idnew") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FULL NAME">
                                <ItemTemplate>
                                    <%# Eval("NewFullName")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CERTICATES">
                                <ItemTemplate>
                                    <asp:Repeater ID="rptinner" runat="server">
                                        <ItemTemplate>
                                            <div class="" style="padding: 2px!important; border: 1px solid #ccc; margin: 0px 2px;
                                                display: inline-table;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td colspan="2" style="padding: 2px!important;">
                                                            <img src='<%# Eval("Image").ToString().Length > 0 ?(DbHelper.Configuration.serverAddress+ "Member/resizeImage.ashx?w=200&h=200&record=" + Eval("Image").ToString()) : string.Empty %>'
                                                                alt="" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="clearfix" style="float: left; padding: 2px 4px;">
                                                            <a href='/Files/<%# Eval("Image") %>' target="_blank" class="btn btn-sm btn-sm1 btn-primary">
                                                                View</a>
                                                        </td>
                                                        <td class="clearfix" style="float: right; padding: 2px 4px;">
                                                            <a href="javascript:;" onclick="LoadDetail('<%# Eval("UniqueID") %>');" class="btn btn-sm btn-sm1 btn-primary">
                                                                Update</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="modal fade" id="edit_cer_modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">
                        Edit Certificate</h4>
                </div>
                <div class="modal-body">
                    <div class="msg-container">
                        <asp:PlaceHolder ID="msgmodal" runat="server"></asp:PlaceHolder>
                    </div>
                    <div class="formRow">
                        <div class="span5" style="margin: 0px;">
                            <span class="span2"><b>Upload Degree/Certificate:</b></span>
                            <div class="control-group">
                                <div class="span2">
                                    <asp:FileUpload ID="txtdegcer" runat="server" CssClass="input-sm btn-file" Style="margin-left: 20px;
                                        width: 165px;" />
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div style="float: right;">
                        <button type="button" class="btn btn-default" data-dismiss="modal">
                            Close</button>
                        &nbsp;
                        <button type="button" class="btn btn-primary" onclick="FileUpload();return false;">
                            Update</button>
                        <%--<asp:Button ID="btnSaveRefer" runat="server" class="btn btn-primary" Text="Update" OnClick="FileUpload();return false;">--%>
                        <%--</asp:Button>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdatePanelProgress" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="loading_message_mask">
            </div>
            <div class="loading_message">
                Please Wait...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="txtuniqueid" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function LoadDetail(uniqueid) {
            $('#<%= txtuniqueid.ClientID %>').val(uniqueid);
            $('#edit_cer_modal').modal('show').off('hidden.bs.modal');
        }
        function close_modal() {
            $('#edit_cer_modal').modal('hide').on('hidden.bs.modal');
        }

        function FileUpload() {
            var imagefile = $('#<%=txtdegcer.ClientID%>').val();
            if (imagefile.length > 0) {
                $("#edit_cer_modal div.msg-container").html('');
                var _id = $('#<%= txtuniqueid.ClientID %>').val();
                var uploadfiles = $("#<%= txtdegcer.ClientID %>").get(0);
                var uploadedfiles = uploadfiles.files;
                var fromdata = new FormData();
                for (var i = 0; i < uploadedfiles.length; i++) {
                    fromdata.append(uploadedfiles[i].name, uploadedfiles[i]);
                }
                var choice = {};
                choice.url = "/Member/FileUploadHandler.ashx?record=" + _id + "";
                choice.type = "POST";
                choice.data = fromdata;
                choice.contentType = false;
                choice.processData = false;
                choice.success = function (result) {
                    if (result == "OK") {
                        $('#edit_cer_modal').modal('hide').on('hidden.bs.modal');
                        alert('Certificate Updated Successfully');
                        window.location.reload();

                    }
                    else {
                        AlertMessage("#edit_cer_modal div.msg-container", "Unable to process, please try again.", 2);
                    }
                };

                $.ajax(choice);
                event.preventDefault();
            }
            else {
                AlertMessage("#edit_cer_modal div.msg-container", "Please select file to upload.", 2);
            }
        }

    </script>
</asp:Content>
