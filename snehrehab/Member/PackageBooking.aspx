<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_PackageBooking" Title="" Codebehind="PackageBooking.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
.my-checkbox{}
.my-checkbox input[type='checkbox']{padding: 0;margin: 0;margin-top: -3px;}
.my-checkbox label{display: inline-block;float: none;width: auto;margin: 0px;padding: 0px;margin-left: 10px;margin-top: 4px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
               New Package Booking :
            </div>
            <div class="pull-right">
                <a href="/Member/PackageBookings.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Select Patient:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePatient" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtPatient" runat="server" CssClass="chzn-select span4" AutoPostBack="True"
                                    OnSelectedIndexChanged="txtPatient_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
            <div class="span12">
            <asp:UpdatePanel ID="UpdatDetail" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="PatientGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                        AutoGenerateColumns="false">
                        <EmptyDataTemplate>
                            Patient registration detail not found...</EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="REG DATE"><ItemTemplate><%# FORMATDATE(Eval("RegistrationDate").ToString())%></ItemTemplate><HeaderStyle Width="85px" /></asp:TemplateField>
                            <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%# Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                            <asp:BoundField HeaderText="TELEPHONE" DataField="TelephoneNo" />
                            <asp:BoundField HeaderText="ADDRESS" DataField="rAddress" />
                            <asp:BoundField HeaderText="CITY" DataField="CityName" />
                            <asp:TemplateField HeaderText="BIRTH DATE"><ItemTemplate><%# FORMATDATE(Eval("BirthDate").ToString())%></ItemTemplate><HeaderStyle Width="85px" /></asp:TemplateField>
                            <asp:TemplateField HeaderText="TYPE"><ItemTemplate><%#Eval("PatientType")%></ItemTemplate><HeaderStyle Width="125px" /></asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
             <div class="clearfix">
             </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Select Session:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateSession" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtSession" runat="server" CssClass="chzn-select span4" AutoPostBack="True"
                                    OnSelectedIndexChanged="txtSession_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Select Package:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePackage" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtPackage" runat="server" CssClass="chzn-select span4" AutoPostBack="True"
                                    OnSelectedIndexChanged="txtPackage_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Appointment Charge:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateAppointmentCharge" runat="server">
                            <ContentTemplate><%--ReadOnly="true"--%>
                                <asp:TextBox ID="txtAppointmentCharge" runat="server" CssClass="span4" onkeyup="PackageAmount();"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Appointment Count:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate><%--ReadOnly="true"--%>
                                <asp:TextBox ID="txtAppointmentCount" runat="server" CssClass="span4" onkeyup="PackageAmount();"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Package Amount:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePackageAmount" runat="server">
                            <ContentTemplate><%--ReadOnly="true"--%>
                                <asp:TextBox ID="txtPackageAmount" runat="server" CssClass="span4" ></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Apply Discount:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <div class="span1" style="margin-left: 0px;">
                                    <asp:CheckBox ID="txtPackageIsDiscounted" runat="server" Text=" Yes" CssClass="my-checkbox"
                                        AutoPostBack="true" OnCheckedChanged="txtPackageIsDiscounted_CheckedChanged" />
                                </div>
                                <div class="span3">
                                    <asp:DropDownList ID="txtPackageDiscountedOn" runat="server" 
                                        CssClass="chzn-select" Width="100%" AutoPostBack="True" 
                                        onselectedindexchanged="txtPackageDiscountedOn_SelectedIndexChanged" Enabled="false">
                                        <asp:ListItem Value="-1">Select Discount</asp:ListItem>
                                        <asp:ListItem Value="1">Discount In Session</asp:ListItem>
                                        <asp:ListItem Value="2">Discount In Package</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Discount Type:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtPackageDiscountType" runat="server" CssClass="chzn-select span2"
                                    AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="txtPackageDiscountType_SelectedIndexChanged">
                                    <asp:ListItem Value="-1">Discount Type</asp:ListItem>
                                    <asp:ListItem Value="1">Percent (%)</asp:ListItem>
                                    <asp:ListItem Value="2">Rupees (INR)</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                <ContentTemplate>
                    <div id="tbPackageDiscount" runat="server" visible="false">
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    Discount Value:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtPackageDiscountValue" runat="server" CssClass="span4" AutoPostBack="True"
                                        OnTextChanged="txtPackageDiscountValue_TextChanged"></asp:TextBox>
                                </div>
                            </div>
                            <div class="span6">
                                <label class="control-label">
                                    Net Payable:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtPackageNetAmt" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </div>
                    <div id="tbPackageDiscountSessionCh" runat="server" visible="false">
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    New Session Charge:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtPackageSessionChargeNew" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <hr />
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Mode of Payment:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePaymentMode" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtPaymentMode" runat="server" CssClass="chzn-select span4"
                                    OnSelectedIndexChanged="txtPaymentMode_SelectedIndexChanged" AutoPostBack="true">
                                   
                                    <asp:ListItem Value="1">Cash</asp:ListItem>
                                    <asp:ListItem Value="2">Credit</asp:ListItem>
                                    <asp:ListItem Value="3">Cheque</asp:ListItem>
                                    <asp:ListItem Value="4">Online</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Booking Date:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtBookingDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <asp:UpdatePanel ID="UpdateChequeDetail" runat="server">
                <ContentTemplate>
                    <div id="tab_Cheque" runat="server" visible="false">
                    <div class="formRow">
                        <div class="span6">
                            <label class="control-label">
                                Bank Name:</label>
                            <div class="control-group">
                                <asp:DropDownList ID="txtBankName" runat="server" CssClass="chzn-select span4">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="span6">
                            <label class="control-label">
                                Branch Name:</label>
                            <div class="control-group">
                                <asp:TextBox ID="txtBranchName" runat="server" CssClass="span4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="span6">
                            <label class="control-label">
                                Cheque No.:</label>
                            <div class="control-group">
                                <asp:TextBox ID="txtChequeNo" runat="server" CssClass="span4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="span6">
                            <label class="control-label">
                                Cheque Date:</label>
                            <div class="control-group">
                                 <asp:TextBox ID="txtChequeDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    </div>
                    <div id="tab_online" runat="server" visible="false">
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    Transaction ID:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtTransactionID" runat="server" CssClass="span4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="span6">
                                <label class="control-label">
                                    Transaction Date:</label>
                                <div class="control-group">
                                     <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                    <div id="tab_cash_Credit" runat="server" visible="true">
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    Hospital Receipt ID:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtHospitalReceiptID" runat="server" CssClass="span4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="span6">
                                <label class="control-label">
                                    Hospital Receipt Date:</label>
                                <div class="control-group">
                                     <asp:TextBox ID="txtHospitalReceiptDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Narration:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtNarration" runat="server" CssClass="span4" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        &nbsp;</label>
                    <div class="control-group">
                        <div id="black_loader_package"></div>
                        <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-danger" onclick="btnSave_Click" OnClientClick="if(confirm('Are you sure to book new package..??')){ShowBlakLoader('black_loader_package');DisableOnSubmit(this);return true;}else{return false;}"></asp:LinkButton>
                        &nbsp;
                        <a href="/Member/PackageBookings.aspx" class="btn btn-default">Cancel</a>
                    </div>
                </div>
                <div class="clearfix">
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
    <script type="text/javascript">
        function PackageAmount() {
            var _chrg = parseFloat($('#<%=txtAppointmentCharge.ClientID %>').val()); if (isNaN(_chrg)) { _chrg = 0; }
            var _count = parseFloat($('#<%=txtAppointmentCount.ClientID %>').val()); if (isNaN(_count)) { _count = 0; }
            var _total = _chrg * _count;
            $('#<%= txtPackageNetAmt.ClientID %>').val('');
            if (_total > 0) {
                $('#<%= txtPackageAmount.ClientID %>').val(_total);
                var ctl = $('#<%= txtPackageIsDiscounted.ClientID %>');
                if (ctl[0].checked) {
                    var _discType = parseInt($('#<%= txtPackageDiscountType.ClientID %>').val()); if (isNaN(_discType)) { _discType = 0; }
                    if (_discType > 0) {
                        var _discVal = parseFloat($('#<%= txtPackageDiscountValue.ClientID %>').val()); if (isNaN(_discVal)) { _discVal = 0; }
                        if (_discType == 1) {
                            $('#<%= txtPackageNetAmt.ClientID %>').val((_total * _discVal / 100).toFixed(2));
                        }
                        if (_discType == 2) {
                            $('#<%= txtPackageNetAmt.ClientID %>').val(_total - _discVal);
                        }
                        var _discOn = parseInt($('#<%= txtPackageDiscountedOn.ClientID %>').val()); if (isNaN(_discOn)) { _discOn = 0; }
                        if (_discOn == 1) {
                            var _netAmt = parseFloat($('#<%= txtPackageNetAmt.ClientID %>').val()); if (isNaN(_netAmt)) { _netAmt = 0; }
                            $('#<%= txtPackageSessionChargeNew.ClientID %>').val((_netAmt / _count).toFixed(2));
                        }
                    }
                }
            }
            else {
                $('#<%=txtPackageAmount.ClientID %>').val('');
            }
        }
    </script>
</asp:Content>

