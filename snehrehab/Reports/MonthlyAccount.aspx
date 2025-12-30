<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true"
    CodeBehind="MonthlyAccount.aspx.cs" Inherits="snehrehab.Reports.MonthlyAccount"
    Title="" %>

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
    function OtherAccount(a, b) {
        $('#myModal div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Loading Please Wait...</span>');
        $.ajax({
            type: "POST", url: "/Snehrehab.asmx/Monthly_OtherAccount", data: "{'d':'" + a + "','id':'" + b + "'}",
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (response) {
                $('#myModal div.modal-body').html(response.d);
            },
            error: function (msg) {
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
                Monthly Account Report :</div>
            <div class="pull-right">
                <a href="/Reports/Account.aspx" class="btn btn-primary">Report List</a>
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
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <div style="white-space: nowrap; overflow-x: auto;">
                <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                    AutoGenerateColumns="false" OnPageIndexChanging="ReportGV_PageIndexChanging"
                    PageSize="31" AllowPaging="true" OnRowDataBound="ReportGV_RowDataBound" ShowFooter="true">
                    <EmptyDataTemplate>
                        No records found...</EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="DATE">
                            <ItemTemplate>
                                <%# FORMATDATE(Eval("PayDate").ToString())%></ItemTemplate>
                            <HeaderStyle Width="85px" />
                            <FooterTemplate>
                                <b>Total:</b></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TOTAL">
                            <ItemTemplate>
                                <%#GetText(Eval("TotalAmt").ToString(), Eval("PayDate").ToString(), "1")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>

                      <%--  <asp:TemplateField HeaderText="Sp.H.V.">
                            <ItemTemplate>
                                <%#GetText(Eval("SpeechHomeVisitEvaluation").ToString(), Eval("PayDate").ToString(), "25")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblspeechhomevisiteval" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>--%>

                       <%-- <asp:TemplateField HeaderText="Audiology">
                            <ItemTemplate><%#GetText(Eval("Audiology").ToString(), Eval("PayDate").ToString(), "26")%></ItemTemplate>
                            <FooterTemplate><asp:Label ID="lblAudiology" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>

                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="MATRIX" Visible="true">
                            <ItemTemplate>
                                <%#GetText(Eval("MatrixAmt").ToString(), Eval("PayDate").ToString(), "10")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblMatrix" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MVPT" Visible="true">
                            <ItemTemplate>
                                <%#GetText(Eval("MVPTAmt").ToString(), Eval("PayDate").ToString(), "11")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblMVPT" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HOME VISIT">
                            <ItemTemplate>
                                <%#GetText(Eval("HomeVisitEvaluation").ToString(), Eval("PayDate").ToString(), "21")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblhomevisit" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EXP" Visible="false">
                            <ItemTemplate>
                                <%#GetText(Eval("ExpAmt").ToString(), Eval("PayDate").ToString(), "2")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblExp" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OTHER CASH">
                            <ItemTemplate>
                                <%#GetText(Eval("CashEntry").ToString(), Eval("PayDate").ToString(), "3")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblOtherCash" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="OTHER ACTIVITIES">
                            <ItemTemplate>
                                <%#GetText(Eval("OTHER_ACTIVITIES").ToString(), Eval("PayDate").ToString(), "20")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblOtherActivity" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sp. T.">
                            <ItemTemplate>
                                <%#GetText(Eval("SpeechAmt").ToString(), Eval("PayDate").ToString(), "7")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblSt" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ONLINE">
                            <ItemTemplate>
                                <%#GetText(Eval("OnlineAmt").ToString(), Eval("PayDate").ToString(), "22")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblonlineamt" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        
                       <asp:TemplateField HeaderText="TOTAL CASH">
                            <ItemTemplate>
                                <%#GetText(Eval("TOTAL_CASH").ToString(), Eval("PayDate").ToString(), "19")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotalCash" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                       
                        
                        <asp:TemplateField HeaderText="C. P." Visible="false">
                            <ItemTemplate>
                                <%#GetText(Eval("ClinicalAmt").ToString(), Eval("PayDate").ToString(), "6")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblCp" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="NOPD" Visible="false">
                            <ItemTemplate>
                                <%#GetText(Eval("NopdAmt").ToString(), Eval("PayDate").ToString(), "8")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblNopd" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DIET" Visible="false">
                            <ItemTemplate>
                                <%#GetText(Eval("DietAmt").ToString(), Eval("PayDate").ToString(), "9")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblDiet" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="V. T." Visible="false">
                            <ItemTemplate>
                                <%#GetText(Eval("VisionTherapyAmt").ToString(), Eval("PayDate").ToString(), "14")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblVision" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WALK AID" Visible="false">
                            <ItemTemplate>
                                <%#GetText(Eval("WalkAidAmt").ToString(), Eval("PayDate").ToString(), "15")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblWalkAid" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="S. EDU" Visible="false">
                            <ItemTemplate>
                                <%#GetText(Eval("SpecialEdu").ToString(), Eval("PayDate").ToString(), "16")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblSpecialEdu" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CHEQUE" Visible="false">
                            <ItemTemplate>
                                <%#GetText(Eval("CheqAmt").ToString(), Eval("PayDate").ToString(), "12")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblCheque" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CASH"  Visible="false">
                            <ItemTemplate>
                                <%#GetText(Eval("CashAmt").ToString(), Eval("PayDate").ToString(), "13")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblCash" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
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
                        Monthly Account Detail</h5>
                </div>
                <div class="modal-body">
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
