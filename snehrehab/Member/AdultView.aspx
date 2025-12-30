<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="AdultView.aspx.cs" Inherits="snehrehab.Member.AdultView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(function () {
        $('.grid-content input[type="text"]').attr('disabled', 'disabled');
        $('.grid-content textarea').attr('disabled', 'disabled');
        $('.grid-content select').attr('disabled', 'disabled');
    });
</script>
<style type="text/css">
    input[disabled], select[disabled], textarea[disabled], input[readonly], select[readonly], textarea[readonly] {cursor: pointer;background-color: #ffffff;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                <%= _headerText %>
                :
            </div>
            <div class="pull-right">
                <a href="/Member/Patients.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content" style="padding: 0px;">
            <ajaxToolkit:TabContainer ID="tb_Contents" runat="server" CssClass="fancy fancy-green">
                <ajaxToolkit:TabPanel ID="tb_Personal" runat="server" HeaderText="Personal">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Patient Code:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtPatientCode" runat="server" CssClass="span3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Full Name:</label>
                                    <div class="control-group">
                                        <asp:DropDownList ID="txtSaluteName" runat="server" CssClass="span1">
                                            <asp:ListItem Value="">- - -</asp:ListItem>
                                            <asp:ListItem Value="MR">Mr</asp:ListItem>
                                            <asp:ListItem Value="MS">Ms</asp:ListItem>
                                            <asp:ListItem Value="MAST">Mast</asp:ListItem>
                                            <asp:ListItem Value="MISS">Miss</asp:ListItem>
                                        </asp:DropDownList>
                                        <div class="span3">
                                            <asp:TextBox ID="txtFullName" runat="server" CssClass="span3 capitalize"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Date of Birth:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtDob" runat="server" CssClass="span2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Age:</label>
                                    <div class="control-group">
                                        <div class="span2" style="margin-left: 0; max-width: 120px;">
                                            <asp:TextBox ID="txtAge" runat="server" CssClass="span1"></asp:TextBox>
                                            &nbsp;Year(s)
                                        </div>
                                        <div class="span2" style="margin-left: 0; max-width: 120px;">
                                            <asp:TextBox ID="txtMonths" runat="server" CssClass="span1"></asp:TextBox>
                                            &nbsp;Month(s)
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Sex:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtSex" runat="server" CssClass="span2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Personal Mobile No:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Telephone No:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtTelephoneNo" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        E-Mail ID :</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtMail" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Address" runat="server" HeaderText="Address">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Residental Address:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtResidental" runat="server" CssClass="span4" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Correspondence Address:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtCorrespondence" runat="server" CssClass="span4" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Category :</label>
                                    <div class="control-group">
                                        <asp:DropDownList ID="txtCategory" runat="server" CssClass="span4">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Country :</label>
                                    <div class="control-group">
                                        <asp:UpdatePanel ID="UpdateCountry" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="txtCountry" runat="server" CssClass="span4">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> State :</label>
                                    <div class="control-group">
                                        <asp:UpdatePanel ID="UpdateState" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="txtState" runat="server" CssClass="span4">
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
                                        <span class="req_red">*</span> City :</label>
                                    <div class="control-group">
                                        <asp:UpdatePanel ID="UpdateCity" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="txtCity" runat="server" CssClass="span4">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Zip Code :</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Registration" runat="server" HeaderText="Registration">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Registration Date:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtRegistrationDate" runat="server" CssClass="span2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Registration Code:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtRegistrationCode" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                       <span class="req_red">*</span> Referred By:</label>
                                    <div class="control-group">
                                        <asp:DropDownList ID="txtReferred" runat="server" CssClass="span4">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Consulted By:</label>
                                    <div class="control-group">
                                        <asp:DropDownList ID="txtConsulted" runat="server" CssClass="span4">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Visit Purpose:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtVisitPurpose" runat="server" CssClass="span4" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Medical History:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtMedicalHistory" runat="server" CssClass="span4" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Medical" runat="server" HeaderText="Medical">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Adult Case:</label>
                                    <div class="control-group">
                                        <asp:DropDownList ID="txtAdultCase" runat="server" CssClass="span4">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Preferred time for therapy</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtPreferredTime" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Investigation Done:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtInvestigation" runat="server" CssClass="span4" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Medical advice or treatment:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtMedicalAdvice" runat="server" CssClass="span4" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Chief Complaints:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtChiefComplaints" runat="server" CssClass="span4" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Brief History:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtBriefHistory" runat="server" CssClass="span4" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Payment Type:</label>
                                    <div class="control-group">
                                        <asp:DropDownList ID="txtPaymentType" runat="server" CssClass="span4">
                                            <asp:ListItem Value="1">Cash</asp:ListItem>
                                            <asp:ListItem Value="2">Credit</asp:ListItem>
                                            <asp:ListItem Value="3">Cheque</asp:ListItem>
                                            <asp:ListItem Value="4">Online</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow " id="Cheqfields" runat="server">
                                <div class="span6">
                                    <label class="control-label">
                                        Bank Name:</label>
                                    <div class="control-group">
                                        <asp:DropDownList ID="txtPaymentBankName" runat="server" CssClass="span4">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Bank Branch:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtBankBranch" CssClass="span4" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Cheque No:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtCheqNo" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div> 
                                <div class="span6">
                                    <label class="control-label">
                                        Cheque Date:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtChequeDate" CssClass="span2" runat="server"></asp:TextBox>
                                    </div>
                                </div>                                
                            </div> 
                            <div class="formRow " id="OnlineField" runat="server">
                                <div class="span6">
                                    <label class="control-label">
                                        Transaction ID:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtTransactionID" CssClass="span4" runat="server"></asp:TextBox>
                                    </div>
                                </div> 
                                <div class="span6">
                                    <label class="control-label">
                                        Transaction Date:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="span2"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Narration:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtNarration" runat="server" CssClass="span4" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_photo" runat="server" HeaderText="Upload Photo">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                    </label>
                                    <div class="control-group">
                                        <asp:Image ID="imgpic" runat="server" class="img-responsive" Width="150px" Height="150px" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
            <div class="clearfix">
            </div>
        </div>
    </div>
</asp:Content>
