<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="YearlyAccount.aspx.cs" Inherits="snehrehab.Reports.YearlyAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Yearly Account Report :
            </div>
            <div class="pull-right">
                <a href="/Reports/Account.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div style="float: left;">
                    <div class="span2" style="width: 100px; margin: 0px;">
                        <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                    </div>
                    <div class="span2" style="width: 100px; margin-left: 15px; margin-right: 15px;">
                        <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
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
                    <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="DATE">
                            <ItemTemplate><%# DateName(Eval("PayDate").ToString())%></ItemTemplate>
                            <HeaderStyle Width="100px" />
                            <FooterTemplate><b>Total:</b></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TOTAL">
                            <ItemTemplate><%#GetText(Eval("TotalAmt").ToString(), Eval("PayDate").ToString(), "1")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EXP">
                            <ItemTemplate><%#GetText(Eval("ExpAmt").ToString(), Eval("PayDate").ToString(), "2")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblExp" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OTHER CASH">
                            <ItemTemplate><%#GetText(Eval("CashEntry").ToString(), Eval("PayDate").ToString(), "3")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblOtherCash" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="C. P.">
                            <ItemTemplate><%#GetText(Eval("ClinicalAmt").ToString(), Eval("PayDate").ToString(), "6")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblCp" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sp. T.">
                            <ItemTemplate><%#GetText(Eval("SpeechAmt").ToString(), Eval("PayDate").ToString(), "7")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblSt" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NOPD">
                            <ItemTemplate><%#GetText(Eval("NopdAmt").ToString(), Eval("PayDate").ToString(), "8")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblNopd" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DIET">
                            <ItemTemplate><%#GetText(Eval("DietAmt").ToString(), Eval("PayDate").ToString(), "9")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblDiet" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MATRIX">
                            <ItemTemplate><%#GetText(Eval("MatrixAmt").ToString(), Eval("PayDate").ToString(), "10")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblMatrix" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="V. T.">
                            <ItemTemplate><%#GetText(Eval("VisionTherapyAmt").ToString(), Eval("PayDate").ToString(), "14")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblVision" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P. O.">
                            <ItemTemplate><%#GetText(Eval("PaediatricOrthopaedicAmt").ToString(), Eval("PayDate").ToString(), "22")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblPaediatricOrthopaedic" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="M.T">
                            <ItemTemplate><%#GetText(Eval("MusicTherapy").ToString(), Eval("PayDate").ToString(), "23")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblmusictherapy" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WALK AID">
                            <ItemTemplate><%#GetText(Eval("WalkAidAmt").ToString(), Eval("PayDate").ToString(), "15")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblWalkAid" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="S. EDU">
                            <ItemTemplate><%#GetText(Eval("SpecialEdu").ToString(), Eval("PayDate").ToString(), "16")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblSpecialEdu" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="H. V.">
                            <ItemTemplate><%#GetText(Eval("HomeVisitEvaluation").ToString(), Eval("PayDate").ToString(), "17")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblHomeVisitEvaluation" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="H. P.">
                            <ItemTemplate><%#GetText(Eval("HomeProgramm").ToString(), Eval("PayDate").ToString(), "19")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblHomeProgramm" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A. B. T.">
                            <ItemTemplate><%#GetText(Eval("ArtBasedTherapy").ToString(), Eval("PayDate").ToString(), "18")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblArtBasedTherapy" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sp.H.V.">
                            <ItemTemplate><%#GetText(Eval("SpeechHomeVisitEvaluation").ToString(), Eval("PayDate").ToString(), "25")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblspeechhomevisiteval" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CHEQUE">
                            <ItemTemplate><%#GetText(Eval("CheqAmt").ToString(), Eval("PayDate").ToString(), "12")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblCheque" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ONLINE">
                            <ItemTemplate><%#GetText(Eval("OnlineAmt").ToString(), Eval("PayDate").ToString(), "20")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblOnline" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CASH">
                            <ItemTemplate><%#GetText(Eval("CashAmt").ToString(), Eval("PayDate").ToString(), "13")%></ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblCash" runat="server" Text="0" Font-Bold="true"></asp:Label></FooterTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
