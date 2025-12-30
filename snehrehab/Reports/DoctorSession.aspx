<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="DoctorSession.aspx.cs" Inherits="snehrehab.Reports.DoctorSession" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <script type="text/javascript">
        function LoadAccount(a, b) {
            $('#myModal div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Loading Please Wait...</span>');
            $.ajax({
                type: "POST", url: "/Snehrehab.asmx/MonthlyAccount", data: "{'d':'" + a + "','t':'" + b + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function(response) {
                    $('#myModal div.modal-body').html(response.d);
                },
                error: function(msg) {
                    $('#myModal div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Error: ' + msg.statusText + '</span>');
                }
            });
        }
        function DoctorAccount(a, b) {
            $('#myModal div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Loading Please Wait...</span>');
            $.ajax({
                type: "POST", url: "/Snehrehab.asmx/MonthlyAccount_Doctor", data: "{'d':'" + a + "','id':'" + b + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function(response) {
                    $('#myModal div.modal-body').html(response.d);
                },
                error: function(msg) {
                    $('#myModal div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Error: ' + msg.statusText + '</span>');
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Doctor Session :</div>
            <div class="pull-right">
                <a href="/Reports/AList.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div style="float: left;">
                    <div class="span2" style="width: 100px; margin: 0px;">
                        <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date"
                            Width="100px"></asp:TextBox>
                    </div>
                    <div class="span2" style="width: 100px; margin-left: 15px; margin-right: 15px;">
                        <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date"
                            Width="100px"></asp:TextBox>
                    </div>
                </div>
                <div style="float: left; margin-top: 4px;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click"
                        Visible="true">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <div style="white-space: nowrap; overflow-x: auto;">
                <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered" 
                    PagerStyle-CssClass="custome-pagination" OnPageIndexChanging="ReportGV_PageIndexChanging"
                    PageSize="31" AllowPaging="true" ShowFooter="true" 
                    OnRowDataBound="ReportGV_RowDataBound">

<PagerStyle CssClass="custome-pagination"></PagerStyle>
                    <EmptyDataTemplate>
                        No records found...</EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="SR NO">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %></ItemTemplate>
                            <HeaderStyle Width="40px" />
                        </asp:TemplateField>
                      
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
                       Doctor Session
                    </h5>
                </div>
                <div class="modal-body">
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
