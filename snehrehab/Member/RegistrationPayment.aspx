<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_RegistrationPayment" Title="" Codebehind="RegistrationPayment.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Balance Reg. Charges:
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <asp:GridView ID="PatientGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="PatientGV_PageIndexChanging"
                PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>
                    No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="SR NO">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %></ItemTemplate>
                        <HeaderStyle Width="40px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="REG DATE">
                        <ItemTemplate>
                            <%# FORMATDATE(Eval("RegistrationDate").ToString())%></ItemTemplate>
                        <HeaderStyle Width="85px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FULL NAME">
                        <ItemTemplate>
                            <%# Eval("FullName").ToString()%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="ADDRESS" DataField="rAddress" />
                    <asp:BoundField HeaderText="CITY" DataField="CityName" />
                    <asp:TemplateField HeaderText="BIRTH DATE">
                        <ItemTemplate>
                            <%# FORMATDATE(Eval("BirthDate").ToString())%></ItemTemplate>
                        <HeaderStyle Width="85px" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="TYPE" DataField="PatientType" HeaderStyle-Width="125px" />
                    <asp:BoundField HeaderText="AMT" DataField="CreditAmt" />
                    <asp:TemplateField HeaderText="ACTION">
                        <ItemTemplate>
                            <a href="javascript:;" data-toggle="modal" data-target="#myModal" onclick='LoadPayment("<%#Eval("UniqueID") %>", <%#Eval("CreditAmt") %>);'>
                                Pay</a>
                        </ItemTemplate>
                        <ItemStyle CssClass="text-center" />
                        <HeaderStyle CssClass="text-center" Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">
                        Pay Charges</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePayBlock" runat="server">
                        <ContentTemplate>
                            <asp:PlaceHolder ID="txtMsg" runat="server"></asp:PlaceHolder>
                            <div class="formRow">
                                <div class="span5" style="margin: 0px;">
                                    <span class="span2"><b>Payment Mode:</b></span>
                                    <div class="control-group">
                                        <div class="span2">
                                            <asp:DropDownList ID="txtPaymentMode" runat="server" CssClass="chzn-select span2"
                                                OnSelectedIndexChanged="txtPaymentMode_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="1">Cash</asp:ListItem>
                                                <%--<asp:ListItem Value="2">Credit</asp:ListItem>--%>
                                                <%--<asp:ListItem Value="3">Cheque</asp:ListItem>--%>
                                                <asp:ListItem Value="4">Online</asp:ListItem>
                                                <asp:ListItem Value="100">Bulk Package</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div id="tab_online" runat="server" visible="false">
                                <div class="formRow">
                                    <div class="span5" style="margin: 0px;">
                                        <span class="span2"><b>Transaction ID:</b></span>
                                        <div class="control-group">
                                            <div class="span2">
                                                <asp:TextBox ID="txtTransactionID" runat="server" CssClass="span3"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                                <div class="formRow">
                                    <div class="span5" style="margin: 0px;">
                                        <span class="span2"><b>Transaction Date:</b></span>
                                        <div class="control-group">
                                            <div class="span2">
                                                <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </div>
                            <div id="tab_Bulk" runat="server" visible="false">
                                <div class="formRow">
                                    <div class="span5" style="margin: 0px;">
                                        <span class="span2"><b>Bulk Amount:</b></span>
                                        <div class="control-group">
                                            <div class="span2">
                                                <asp:TextBox ID="txtbulkamount" runat="server" CssClass="span3"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>                                
                                
                            </div>
                            <div class="formRow">
                                <div class="span5" style="margin: 0px;">
                                    <span class="span2"><b>Payment Amount:</b></span>
                                    <div class="control-group">
                                        <div class="span2">
                                            <asp:TextBox ID="txtPaymentAmount" runat="server" CssClass="span2"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span5" style="margin: 0px;">
                                    <span class="span2"><b>Payment Date:</b></span>
                                    <div class="control-group">
                                        <div class="span2">
                                            <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span5" style="margin: 0px;">
                                    <span class="span2"><b>Narration:</b></span>
                                    <div class="control-group">
                                        <div class="span2">
                                            <asp:TextBox ID="txtPaymentNarration" runat="server" CssClass="span3" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <asp:HiddenField ID="txtHiddenPayment" runat="server" />
                            <asp:HiddenField ID="txthidbulkid" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <div style="float: right;">
                        <asp:UpdateProgress ID="UpdateProgressUpdatePayBtn" runat="server" AssociatedUpdatePanelID="UpdatePayBtn"
                            DisplayAfter="0">
                            <ProgressTemplate>
                                <img src="/images/load/1.gif" style="margin-left: 15px; margin-top: 5px;" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    <div style="float: right;">
                        <button type="button" class="btn btn-default" data-dismiss="modal">
                            Close</button>
                        &nbsp;
                        <asp:UpdatePanel ID="UpdatePayBtn" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:LinkButton ID="btnPayAmount" runat="server" CssClass="btn btn-primary" OnClick="btnPayAmount_Click"
                                    OnClientClick="return confirm('Are you sure to pay amount..??');">Pay Now</asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function LoadPayment(_payID, _amt) {
            document.getElementById('<%= txtHiddenPayment.ClientID %>').value = "";
            document.getElementById('<%= txtPaymentAmount.ClientID %>').value = "";
            document.getElementById('<%= txtPaymentDate.ClientID %>').value = "";
            if (_payID.length > 0 && _amt > 0) {
                document.getElementById('<%= txtHiddenPayment.ClientID %>').value = _payID;
                document.getElementById('<%= txtPaymentAmount.ClientID %>').value = _amt;
            }
        }
    </script>
</asp:Content>

