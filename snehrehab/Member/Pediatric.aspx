<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_Pediatric" Title="" CodeBehind="Pediatric.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .add_new {
                margin-left: 11px;
                background: #4e8de0;
                color: #fff;
                padding: 5px 10px;
                font-size: 11px;
                height: 31px;
        }
        a:hover {
        color: #ffffff!important;
        text-decoration: underline;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="grid">
        <asp:UpdatePanel ID="new" runat="server">
        <ContentTemplate><asp:PlaceHolder ID="newmsg" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
        </asp:UpdatePanel>
        <div class="grid-title">
            <div class="pull-left">
                <%= _headerText %>
                :
            </div>
            <div class="pull-right" id="pedlist" runat="server">
                <a href="/Member/Patients.aspx" class="btn btn-primary">View List</a>
            </div>
            <div class="pull-right" id="reflist" runat="server" visible="false">
                <a href="/Reports/ReferrenceList.aspx" class="btn btn-primary">View List</a>
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
                                        <asp:TextBox ID="txtPatientCode" runat="server" CssClass="span3" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        MR No:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtmrno" runat="server" CssClass="span3"></asp:TextBox>
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
                                        <asp:DropDownList ID="txtSaluteName" runat="server" CssClass="span1" onchange="SelectGender();">
                                            <asp:ListItem Value="">- - -</asp:ListItem>
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
                                        <asp:TextBox ID="txtDob" runat="server" CssClass="span2 age-datepicker" onchange="DOBSelected();"></asp:TextBox>
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
                                            <asp:TextBox ID="txtAge" runat="server" CssClass="span1" Enabled="false"></asp:TextBox>
                                            &nbsp;Year(s)
                                        </div>
                                        <div class="span2" style="margin-left: 0; max-width: 120px;">
                                            <asp:TextBox ID="txtMonths" runat="server" CssClass="span1" Enabled="false"></asp:TextBox>
                                            &nbsp;Month(s)
                                        </div>
                                        <asp:HiddenField ID="txthAge" runat="server" />
                                        <asp:HiddenField ID="txthMonth" runat="server" />
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
                                        <asp:TextBox ID="txtSex" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Is the child attending school?:</label>
                                    <div class="control-group">
                                        <asp:DropDownList ID="txtHasSchool" runat="server" CssClass="chzn-select span4" onchange="AttendSchool();">
                                            <asp:ListItem Value="-1">Select</asp:ListItem>
                                            <asp:ListItem Value="true">Yes</asp:ListItem>
                                            <asp:ListItem Value="false">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Schooling Year:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtSchoolingYear" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Name of School:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtSchoolName" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
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
                                    <label classs="control-label">
                                        Diagnosis:
                                    </label>
                                    <div class="control-group">
                                        <asp:ListBox ID="txtDiagnosis" runat="server" SelectionMode="Multiple" CssClass="chzn-select-multi span4" data-placeholder="Select Diagnosis"></asp:ListBox>
                                    </div>
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        Other Diagnosis :
                                    </label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtDiagnosisOther" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                    </label>
                                    <div class="control-group">
                                        <asp:LinkButton ID="btnNext1" runat="server" CssClass="btn btn-danger" OnClick="btnNext1_Click"
                                            OnClientClick="DisableOnSubmit(this);"> Next </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Parent" runat="server" HeaderText="Parent">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Father/Gaurdian Name:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtFatherName" runat="server" CssClass="span4 capitalize"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Occupation:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtFatherOccupation" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        E-Mail ID:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtFatherMailID" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Mobile No:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtFatherMobileNo" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <hr />
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Mother/Gaurdian Name:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtMotherName" runat="server" CssClass="span4 capitalize"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Occupation:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtMotherOccupation" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                        E-Mail ID:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtMotherMailID" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Mobile No:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtMotherMobileNo" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <hr />
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                    </label>
                                    <div class="control-group">
                                        <asp:LinkButton ID="btnPrevious2" runat="server" CssClass="btn btn-default" OnClick="btnPrevious2_Click"> Previous </asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="btnNext2" runat="server" CssClass="btn btn-danger" OnClick="btnNext2_Click"
                                            OnClientClick="DisableOnSubmit(this);"> Next </asp:LinkButton>
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
                                        <asp:DropDownList ID="txtCategory" runat="server" CssClass="chzn-select span4">
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
                                                <asp:DropDownList ID="txtCountry" runat="server" CssClass="chzn-select span4" AutoPostBack="true"
                                                    OnSelectedIndexChanged="txtCountry_SelectedIndexChanged">
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
                                                <asp:DropDownList ID="txtState" runat="server" CssClass="chzn-select span4" AutoPostBack="true"
                                                    OnSelectedIndexChanged="txtState_SelectedIndexChanged">
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
                                                <asp:DropDownList ID="txtCity" runat="server" CssClass="chzn-select span4">
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
                            <hr />
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                    </label>
                                    <div class="control-group">
                                        <asp:LinkButton ID="btnPrevious3" runat="server" CssClass="btn btn-default" OnClick="btnPrevious3_Click"> Previous </asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="btnNext3" runat="server" CssClass="btn btn-danger" OnClick="btnNext3_Click"
                                            OnClientClick="DisableOnSubmit(this);"> Next </asp:LinkButton>
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
                                        <asp:TextBox ID="txtRegistrationDate" runat="server" CssClass="my-datepicker span2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Registration Code:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtRegistrationCode" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <asp:UpdatePanel ID="updatereferby" runat="server">
                                <ContentTemplate>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                <span class="req_red">*</span> Referred By:</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtreferred" runat="server" CssClass="chzn-select span4" AutoPostBack="true"
                                                    OnSelectedIndexChanged="txtreferred_SelectedIndexChanged">
                                                    <asp:ListItem Value="-1">Select Referred By</asp:ListItem>
                                                    <asp:ListItem Value="1">Reference Doctor</asp:ListItem>
                                                    <asp:ListItem Value="2">Reference School</asp:ListItem>
                                                    <asp:ListItem Value="3">Reference Hospital</asp:ListItem>
                                                    <asp:ListItem Value="4">Reference Teacher</asp:ListItem>
                                                    <asp:ListItem Value="5">Reference Other</asp:ListItem>
                                                    <asp:ListItem Value="6">Reference Online</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="span6" id="referselect" runat="server" visible="false" style="display: inline-flex;">
                                            <label class="control-label">
                                                <span class="req_red">*</span> Name:</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtReferSelected" runat="server" CssClass="chzn-select span4"
                                                    Width="291px">
                                                </asp:DropDownList>
                                            </div>
                                            <%--<asp:Button runat="server" ID="btnadd" class="btn btn-primary add_new" OnClientClick="LoadDetail();return false;" Text="Add" />--%>
                                            <input id="btnadd" type="button" value="Add New" onclick="LoadDetail()" class="btn btn-primary add_new"
                                                runat="server" />
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="formRow">                               
                                <div class="span6">
                                    <label class="control-label">
                                        <span class="req_red">*</span> Consulted By:</label>
                                    <div class="control-group">
                                        <asp:DropDownList ID="txtConsulted" runat="server" CssClass="chzn-select span4">
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
                                        Chief Complaints:</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtChiefComplaints" runat="server" CssClass="span4" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
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
                                        Preferred time for therapy</label>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtPreferredTime" runat="server" CssClass="span4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span6">
                                    <label class="control-label">
                                        Payment Type:</label>
                                    <div class="control-group">
                                        <asp:DropDownList ID="txtPaymentType" runat="server" CssClass="chzn-select span4"
                                            AutoPostBack="True" OnSelectedIndexChanged="txtPaymentType_SelectedIndexChanged">
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
                                        <asp:DropDownList ID="txtPaymentBankName" runat="server" CssClass="chzn-select span4">
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
                                        <asp:TextBox ID="txtChequeDate" CssClass="span2 age-datepicker" runat="server"></asp:TextBox>
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
                                        <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="span2 age-datepicker"></asp:TextBox>
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
                            <hr />
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                    </label>
                                    <div class="control-group">
                                        <asp:LinkButton ID="btnPrevious4" runat="server" CssClass="btn btn-default" OnClick="btnPrevious4_Click"> Previous </asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="btnNext4" runat="server" CssClass="btn btn-danger" OnClick="btnNext4_Click"
                                            OnClientClick="DisableOnSubmit(this);"> Next </asp:LinkButton>
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
                                        Select Photo :</label>
                                    <div class="control-group">
                                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="span4" onchange="ShowImagePreview(this);" />
                                    </div>
                                </div>
                                <div class="span6">
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                    </label>
                                    <div class="control-group">
                                        <asp:Image ID="imgpic" runat="server" class="img-responsive" Width="150px" Height="150px" />
                                    </div>
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span6">
                                    <label class="control-label">
                                    </label>
                                    <div class="control-group">
                                        <asp:LinkButton ID="btnPrevious5" runat="server" CssClass="btn btn-default" OnClick="btnPrevious5_OnClick"> Previous </asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-danger" OnClick="btnSave_Click"
                                            OnClientClick="DisableOnSubmit(this);"> Save </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="clearfix">
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
    <div class="modal fade" id="add_new_modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">
                        Add New</h4>
                </div>
                <div class="modal-body">
                    <div class="msg-container">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:PlaceHolder ID="msgmodal" runat="server"></asp:PlaceHolder>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="formRow">
                        <div class="span5" style="margin: 0px;">
                            <span class="span2"><b>Name:</b></span>
                            <div class="control-group">
                                <div class="span2">
                                    <asp:TextBox ID="txtmodname" runat="server" CssClass="span3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="formRow">
                        <div class="span5" style="margin: 0px;">
                            <span class="span2"><b>Contact No.:</b></span>
                            <div class="control-group">
                                <div class="span2">
                                    <asp:TextBox ID="txtmodcontact" onkeypress="CheckNumeric(event);" runat="server"
                                        CssClass="span3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="formRow">
                        <div class="span5" style="margin: 0px;">
                            <span class="span2"><b>Email id:</b></span>
                            <div class="control-group">
                                <div class="span2">
                                    <asp:TextBox ID="txtmodemailid" runat="server" CssClass="span3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="formRow">
                        <div class="span5" style="margin: 0px;">
                            <span class="span2"><b>Website:</b></span>
                            <div class="control-group">
                                <div class="span2">
                                    <asp:TextBox ID="txtmodwebsite" runat="server" CssClass="span3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="formRow">
                        <div class="span5" style="margin: 0px;">
                            <span class="span2"><b>Address:</b></span>
                            <div class="control-group">
                                <div class="span2">
                                    <asp:TextBox ID="txtmodaddress" runat="server" CssClass="span3" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="formRow">
                        <div class="span5" style="margin: 0px;">
                            <span class="span2"><b>Added Date:</b></span>
                            <div class="control-group">
                                <div class="span2">
                                    <asp:TextBox ID="txtmodAddedDate" runat="server" CssClass="span3 my-datepicker"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="modl" runat="server">
                        <ContentTemplate>
                            <div style="float: right;">
                                <button type="button" class="btn btn-default" data-dismiss="modal">
                                    Close</button>
                                &nbsp;
                                <asp:Button ID="btnSaveRefer" runat="server" class="btn btn-primary" OnClick="btnSaveRefer_Click" Text="Save"></asp:Button>
                                <%--<asp:button ID="btnMethod" runat="server" OnClientClick="return formvalidate();"  OnClick="btnMethod_Click" class="btn btn-primary" Text="Update"></asp:button>--%>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="txtreferedbyid" runat="server" />
            <asp:HiddenField ID="txtrefername" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        window.onload = function() {
            SelectGender(); DOBSelected(); AttendSchool();
        }
        function SelectGender() {
            var a = document.getElementById("<%= txtSaluteName.ClientID %>").selectedIndex;
            document.getElementById("<%= txtSex.ClientID %>").value = "";
            if (a == 1 || a == 3) {
                document.getElementById("<%= txtSex.ClientID %>").value = "Male";
            }
            else
                if (a == 2 || a == 4) {
                document.getElementById("<%= txtSex.ClientID %>").value = "Female";
            }
        }
        function AttendSchool() {
            var a = document.getElementById("<%= txtHasSchool.ClientID %>").selectedIndex;
            document.getElementById("<%= txtSchoolingYear.ClientID %>").value = "";
            document.getElementById("<%= txtSchoolName.ClientID %>").value = "";
            if (a == 1) {
                document.getElementById("<%= txtSchoolingYear.ClientID %>").disabled = false;
                document.getElementById("<%= txtSchoolName.ClientID %>").disabled = false;
            }
            else {
                document.getElementById("<%= txtSchoolingYear.ClientID %>").disabled = true;
                document.getElementById("<%= txtSchoolName.ClientID %>").disabled = true;
            }
        }
        function DOBSelected() {
            var a = document.getElementById("<%= txtDob.ClientID %>").value;
            document.getElementById("<%= txtAge.ClientID %>").value = "";
            document.getElementById("<%= txtMonths.ClientID %>").value = "";
            document.getElementById("<%= txthAge.ClientID %>").value = "";
            document.getElementById("<%= txthMonth.ClientID %>").value = "";
            if (a != null) {
                if (a.split('/').length === 3) {
                    var now = new Date();
                    var today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
                    var yearNow = now.getFullYear();
                    var monthNow = now.getMonth() + 1;
                    var dateNow = now.getDate();
                    var dob = new Date(a.substring(6, 10), a.substring(3, 5) - 1, a.substring(0, 2));
                    var yearDob = dob.getFullYear();
                    var monthDob = dob.getMonth() + 1;
                    var dateDob = dob.getDate();

                    yearAge = yearNow - yearDob;
                    if (monthNow >= monthDob)
                        var monthAge = monthNow - monthDob;
                    else {
                        yearAge--;
                        var monthAge = 12 + monthNow - monthDob;
                    }
                    if (dateNow >= dateDob)
                        var dateAge = dateNow - dateDob;
                    else {
                        monthAge--;
                        var dateAge = 31 + dateNow - dateDob;

                        if (monthAge < 0) {
                            monthAge = 11;
                            yearAge--;
                        }
                    }
                    if ((yearAge > 0 || monthAge > 0) && yearAge > -1 && monthAge > -1) {
                        document.getElementById("<%= txtAge.ClientID %>").disabled = false;
                        document.getElementById("<%= txtMonths.ClientID %>").disabled = false;
                        document.getElementById("<%= txtAge.ClientID %>").value = yearAge.toString();
                        document.getElementById("<%= txtMonths.ClientID %>").value = monthAge.toString();
                        try { document.getElementById("<%= txtAge.ClientID %>").setAttribute("value", yearAge.toString()) } catch (e) { }
                        try { document.getElementById("<%= txtMonths.ClientID %>").setAttribute("value", monthAge.toString()) } catch (e) { }
                        try { document.getElementById("<%= txtAge.ClientID %>").innerText = yearAge.toString(); } catch (e) { }
                        try { document.getElementById("<%= txtMonths.ClientID %>").innerText = monthAge.toString(); } catch (e) { }
                        document.getElementById("<%= txtAge.ClientID %>").disabled = true;
                        document.getElementById("<%= txtMonths.ClientID %>").disabled = true;

                        document.getElementById("<%= txthAge.ClientID %>").value = yearAge.toString();
                        document.getElementById("<%= txthMonth.ClientID %>").value = monthAge.toString();
                    }
                }
            }
        } 
    </script>
    <script type="text/javascript">
        function LoadDetail() {
            //var reid = document.getElementById('<%=txtreferedbyid.ClientID%>').value;
            $('#add_new_modal').modal('show').off('hidden.bs.modal')
        }
        function close_modal() {
            $('#add_new_modal').modal('hide').on('hidden.bs.modal', function () {
                $('#<%= txtmodname.ClientID %>').val('');
                $('#<%= txtmodcontact.ClientID %>').val('');
                $('#<%= txtmodemailid.ClientID %>').val('');
                $('#<%= txtmodwebsite.ClientID %>').val('');
                $('#<%= txtmodaddress.ClientID %>').val('');
                $('#<%= txtmodAddedDate.ClientID %>').val('');
            });
        }
        
    </script>

    <script type="text/javascript">
        function CheckNumeric(e) {
            if (window.event) // IE 
            {
                if ((e.keyCode < 48 || e.keyCode > 57) & e.keyCode != 8 & e.keyCode != 46) {
                    event.returnValue = false;
                    return false;
                }
            }
            else { // Fire Fox
                if ((e.which < 48 || e.which > 57) & e.which != 8 & e.keyCode != 46) {
                    e.preventDefault();
                    return false;
                }
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
