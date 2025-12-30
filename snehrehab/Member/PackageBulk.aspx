<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="PackageBulk.aspx.cs" Inherits="snehrehab.Member.PackageBulk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .border
        {
            border: 1px solid #cccccc;
        }
        .my-checkbox input[type='checkbox']
        {
            padding: 0;
            margin: 0;
            margin-top: -3px;
        }
        .my-checkbox label
        {
            display: inline-block;
            float: none;
            width: auto;
            margin: 0px;
            padding: 0px;
            margin-left: 10px;
            margin-top: 4px;
        }
        .checkboes
        {
            margin: 0px;
            max-height: 86px;
            overflow-x: auto;
            padding: 5px;
        }
        .checkboes input[type="checkbox"]
        {
            float: none;
            margin: 0px;
        }
        .checkboes label
        {
            float: none;
            padding: 0px;
            width: auto;
            height: auto;
            display: inline-block;
            margin: 0px;
            line-height: normal;
            margin-bottom: 5px;
            margin-left: 10px;
            padding-top: 4px;
        }
        .d-flex
        {
               display: flex;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Bulk Package Booking :
            </div>
            <div class="pull-right">
                <a href="/Member/PackageBulks.aspx" class="btn btn-primary">View List</a>
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
                                    <asp:TemplateField HeaderText="REG DATE">
                                        <ItemTemplate>
                                            <%# FORMATDATE(Eval("RegistrationDate").ToString())%></ItemTemplate>
                                        <HeaderStyle Width="85px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FULL NAME">
                                        <ItemTemplate>
                                            <%# Eval("FullName").ToString()%></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="TELEPHONE" DataField="TelephoneNo" />
                                    <asp:BoundField HeaderText="ADDRESS" DataField="rAddress" />
                                    <asp:BoundField HeaderText="CITY" DataField="CityName" />
                                    <asp:TemplateField HeaderText="BIRTH DATE">
                                        <ItemTemplate>
                                            <%# FORMATDATE(Eval("BirthDate").ToString())%></ItemTemplate>
                                        <HeaderStyle Width="85px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TYPE">
                                        <ItemTemplate>
                                            <%#Eval("PatientType")%></ItemTemplate>
                                        <HeaderStyle Width="125px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <asp:UpdatePanel ID="updradio" runat="server">
            <ContentTemplate>
            <div class="formRow">
                <div class="span2" style="width: 152px;">
                    <div class="control-group">
                        <asp:RadioButton ID="chkamount" CssClass="radiobutton d-flex" Checked="true" GroupName="Group1" AutoPostBack="true" Text="Amount"   runat="server" OnCheckedChanged="chkamount_CheckedChanged" />
                    </div>
                </div>
                
                <div class="span2" style="width: 152px;">
                    <div class="control-group">
                        <asp:RadioButton ID="chkpackage" CssClass="radiobutton d-flex" GroupName="Group1" AutoPostBack="true" Text="Package"   runat="server" OnCheckedChanged="chkpackage_CheckedChanged" />
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div id="session" runat="server" visible="false">
            <div class="formRow" >
                <div class="span6"  >
                    <label class="control-label">
                        Select Session:</label>
                    <div class="control-group">
                        <div class="span4 checkboes border" style="margin: 0px;">
                            <asp:CheckBoxList ID="txtSession" runat="server" RepeatLayout="Flow" OnSelectedIndexChanged="txtSession_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>
                <div class="span6" id="package" runat="server" visible="false">
                    <label class="control-label">
                        Select Package:</label>
                    <div class="control-group">
                        <div class="span4 checkboes border" style="margin: 0px;">
                            <asp:CheckBoxList ID="txtPackage" runat="server" RepeatLayout="Flow" OnSelectedIndexChanged="txtPackage_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            </div>
            <div id="appointcharg" runat="server" visible="false" >
            <div class="formRow" >
                <div class="span6">
                    <label class="control-label">
                        Appointment Charge:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtAppointmentCharge" runat="server" CssClass="span4" onkeyup="PackageAmount();"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Appointment Count:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtAppointmentCount" runat="server" CssClass="span4" onkeyup="PackageAmount();"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            </div>
            <div id="packageamount" runat="server" visible="false">
            <div class="formRow" >
                <div class="span6">
                    <label class="control-label">
                        Package Amount:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <%--ReadOnly="true"--%>
                                <asp:TextBox ID="txtPackageAmount" runat="server" CssClass="span4"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            </div>
            <div id="amount" runat="server" visible="true">
            <div class="formRow" >
                <div class="span6">
                    <label class="control-label">
                        Amount:</label>
                    <div class="control-group">
                        <%--<asp:UpdatePanel ID="UpdatePackageAmount" runat="server">
                            <ContentTemplate>--%>
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="span4"></asp:TextBox>
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            </div>
            </ContentTemplate></asp:UpdatePanel>
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
                                    <%--<asp:ListItem Value="2">Credit</asp:ListItem>--%>
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
                            <div class="clearfix">
                            </div>
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
                            <div class="clearfix">
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Narration:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtNarration" runat="server" CssClass="span4" TextMode="MultiLine"
                            Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <hr />
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        &nbsp;</label>
                    <div class="control-group">
                        <div id="black_loader_package">
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-danger"
                            OnClick="btnSave_Click" OnClientClick="if(confirm('Are you sure to book bulk package..??')){ShowBlakLoader('black_loader_package');DisableOnSubmit(this);return true;}else{return false;}"></asp:LinkButton>
                        &nbsp; <a href="/Member/PackageBulks.aspx" class="btn btn-default">Cancel</a>
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
   <%-- <script type="text/javascript">
        function PackageAmount() {
            var _chrg = parseFloat($('#<%=txtAppointmentCharge.ClientID %>').val()); if (isNaN(_chrg)) { _chrg = 0; }
            var _count = parseFloat($('#<%=txtAppointmentCount.ClientID %>').val()); if (isNaN(_count)) { _count = 0; }
            var _total = _chrg * _count;
            if (_total > 0) {
                $('#<%= txtPackageAmount.ClientID %>').val(_total);
            }
            else {
                $('#<%=txtPackageAmount.ClientID %>').val('');
            }
        }
    </script>--%>
    <script type="text/javascript">
        function chkamount_Click() {
            var ctl = $('#<%=chkamount.ClientID %>')[0];
            if (ctl.checked) {
                $('#<%=chkpackage.ClientID %>').prop('checked', false);
            }
        }
        function chkpackage_Click() {
            var ctl = $('#<%=chkpackage.ClientID %>')[0];
            if (ctl.checked) {
                $('#<%=chkamount.ClientID %>').prop('checked', false);
            }
        }
    </script>
</asp:Content>
