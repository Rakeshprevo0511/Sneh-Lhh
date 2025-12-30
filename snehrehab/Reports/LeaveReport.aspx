<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true"
    CodeBehind="LeaveReport.aspx.cs" Inherits="snehrehab.Reports.LeaveReport" %>

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
                Leave Report :</div>
            <div class="pull-right">
                <a href="/Reports/AList.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div style="float: left;">
                    <%--<div class="span2" style="width: 100px; margin: 0px;">
                        <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date"
                            Width="100px"></asp:TextBox>
                    </div>
                    <div class="span2" style="width: 100px; margin-left: 15px; margin-right: 15px;">
                        <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date"
                            Width="100px"></asp:TextBox>
                    </div>--%>
                    <div style="float: left;margin-top: 4px;margin-right:20px;">Select Month : </div>
                    <div class="span3" style="width: 120px; margin: 0px;">
                        <asp:TextBox ID="txtOnMonthNew" runat="server" CssClass="month-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                    </div>
                </div>
                <div style="float: left; margin-top: 4px;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
                   &nbsp;
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <div style="white-space: nowrap; overflow-x: auto;">
                <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                    AutoGenerateColumns="false" OnPageIndexChanging="ReportGV_PageIndexChanging"
                    PageSize="31" AllowPaging="true" ShowFooter="true" OnRowDataBound="ReportGV_RowDataBound">
                    <EmptyDataTemplate>
                        No records found...</EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="SR NO">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %></ItemTemplate>
                            <HeaderStyle Width="40px" />
                        </asp:TemplateField>
                        <%-- <asp:BoundField DataField="FullName" HeaderText="Doctor Name" SortExpression="FullName" />
                        <asp:BoundField DataField="1" HeaderText="01" SortExpression="1" />
                        <asp:BoundField DataField="2" HeaderText="02" SortExpression="2" />
                        <asp:BoundField DataField="3" HeaderText="03" SortExpression="3" />
                        <asp:BoundField DataField="4" HeaderText="04" SortExpression="4" />
                         <asp:BoundField DataField="5" HeaderText="05" SortExpression="5" />
                         <asp:BoundField DataField="6" HeaderText="06" SortExpression="6" />
                         <asp:BoundField DataField="7" HeaderText="07" SortExpression="7" />
                         <asp:BoundField DataField="8" HeaderText="08" SortExpression="8" />
                         <asp:BoundField DataField="9" HeaderText="09" SortExpression="9" />
                         <asp:BoundField DataField="10" HeaderText="10" SortExpression="10" />
                         <asp:BoundField DataField="11" HeaderText="11" SortExpression="11" />
                         <asp:BoundField DataField="12" HeaderText="12" SortExpression="12" />
                         <asp:BoundField DataField="13" HeaderText="13" SortExpression="13" />
                         <asp:BoundField DataField="14" HeaderText="14" SortExpression="14" />
                         <asp:BoundField DataField="15" HeaderText="15" SortExpression="15" />
                         <asp:BoundField DataField="16" HeaderText="16" SortExpression="16" />
                         <asp:BoundField DataField="17" HeaderText="17" SortExpression="17" />
                         <asp:BoundField DataField="18" HeaderText="18" SortExpression="18" />
                         <asp:BoundField DataField="19" HeaderText="19" SortExpression="19" />
                         <asp:BoundField DataField="20" HeaderText="20" SortExpression="20" />
                         <asp:BoundField DataField="21" HeaderText="21" SortExpression="21" />
                         <asp:BoundField DataField="22" HeaderText="22" SortExpression="22" />
                         <asp:BoundField DataField="23" HeaderText="23" SortExpression="23" />
                         <asp:BoundField DataField="24" HeaderText="24" SortExpression="24" />
                         <asp:BoundField DataField="25" HeaderText="25" SortExpression="25" />
                         <asp:BoundField DataField="26" HeaderText="26" SortExpression="26" />
                         <asp:BoundField DataField="27" HeaderText="27" SortExpression="27" />
                         <asp:BoundField DataField="28" HeaderText="28" SortExpression="28" />
                         <asp:BoundField DataField="29" HeaderText="29" SortExpression="29" />
                         <asp:BoundField DataField="30" HeaderText="30" SortExpression="30" />
                         <asp:BoundField DataField="31" HeaderText="31" SortExpression="31" />--%>
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
                        Leave Report
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
