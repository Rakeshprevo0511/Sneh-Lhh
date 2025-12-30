<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_BirthDay" Title="" Codebehind="BirthDay.aspx.cs" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function SelectAll(a, ctl) {
            $('.' + a).each(function () {
                $(this).find("input[type='checkbox']").attr('checked', ctl.checked);
            });
            ChkForPatient(); ChkForDoctor(); ChkForManager(); ChkForReception();
        }
        function ChkForPatient() {
            if ($('.patient-div').find('span.patient-select').find("input[type='checkbox']:checked").length > 0) {
                $('.patient-msg').slideDown('slow');
            }
            else {
                $('.patient-msg').slideUp('slow');
            }
        }
        function ChkForDoctor() {
            if ($('.doctor-div').find('span.doctor-select').find("input[type='checkbox']:checked").length > 0) {
                $('.doctor-msg').slideDown('slow');
            }
            else {
                $('.doctor-msg').slideUp('slow');
            }
        }
        function ChkForManager() {
            if ($('.manager-div').find('span.manager-select').find("input[type='checkbox']:checked").length > 0) {
                $('.manager-msg').slideDown('slow');
            }
            else {
                $('.manager-msg').slideUp('slow');
            }
        }
        function ChkForReception() {
            if ($('.reception-div').find('span.reception-select').find("input[type='checkbox']:checked").length > 0) {
                $('.reception-msg').slideDown('slow');
            }
            else {
                $('.reception-msg').slideUp('slow');
            }
        }

        window.onload = function () {
            ChkForPatient(); ChkForDoctor(); ChkForManager(); ChkForReception();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Birth Day Event :
            </div>
        </div>
        <div class="grid-content" style="padding: 0px;">
            <ajaxToolkit:TabContainer ID="tb_Contents" runat="server" CssClass="fancy fancy-green">
                <ajaxToolkit:TabPanel ID="tb_Patient" runat="server" HeaderText="Patient">
                    <ContentTemplate>
                        <div class="patient-div" style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow patient-msg" style="display: none;">
                                <div class="span7">
                                    <asp:TextBox ID="txtPatient" runat="server" CssClass="span7" placeholder="Enter your message"></asp:TextBox>
                                </div>
                                <div style="padding-top: 4px;">
                                    <asp:LinkButton ID="btnPatient" runat="server" Text="Send Message" CssClass="btn btn-danger" OnClick="btnPatient_Click" OnClientClick="DisableOnSubmit(this);"></asp:LinkButton>
                                    <asp:LinkButton ID="btnPatientEmail" runat="server" Text="Send Email" CssClass="btn btn-danger" OnClick="btnPatientEmail_Click" OnClientClick="DisableOnSubmit(this);"></asp:LinkButton>
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <asp:GridView ID="PatientGV" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="txtSelect" runat="server" CssClass="patient-select" onclick="ChkForPatient();" />
                                                    <asp:HiddenField ID="txtMobile" runat="server" Value='<%#Eval("MobileNo") %>' />
                                                    <asp:HiddenField ID="txtEmail" runat="server" Value='<%#Eval("MailID") %>' />

                                                    <asp:HiddenField ID="txtID" runat="server" Value='<%#Eval("ID") %>' />
                                                    <asp:HiddenField ID="txtFullName" runat="server" Value='<%#Eval("FullName") %>' />
                                                    <asp:HiddenField ID="txtBirthDate" runat="server" Value='<%#Eval("BirthDate") %>' />
                                                    <asp:HiddenField ID="txtrAddress" runat="server" Value='<%#Eval("rAddress") %>' />
                                                    <asp:HiddenField ID="txtRegistrationDate" runat="server" Value='<%#Eval("RegistrationDate") %>' />


                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="txtSelectAll" runat="server" onclick="SelectAll('patient-select', this);" />
                                                </HeaderTemplate>
                                                <ItemStyle CssClass="text-center" />
                                                <HeaderStyle CssClass="text-center" Width="35px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FULL NAME">
                                                <ItemTemplate>
                                                    <%#Eval("FullName").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BIRTH DATE">
                                                <ItemTemplate>
                                                    <%# DbHelper.Configuration.FORMATDATE(Eval("BirthDate").ToString())%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="MOBILE NO" DataField="MobileNo" />
                                            <asp:BoundField HeaderText="MAIL ID" DataField="MailID" />

                                            <asp:TemplateField HeaderText="ADDRESS">
                                                <ItemTemplate>
                                                    <%# Eval("rAddress")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="REG. DATE">
                                                <ItemTemplate>
                                                    <%# DbHelper.Configuration.FORMATDATE(Eval("RegistrationDate").ToString())%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No records found...
                                        </EmptyDataTemplate>
                                        <PagerStyle CssClass="custome-pagination" />
                                    </asp:GridView>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Doctor" runat="server" HeaderText="Doctor">
                    <ContentTemplate>
                        <div class="doctor-div" style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow doctor-msg" style="display: none;">
                                <div class="span7">
                                    <asp:TextBox ID="txtDoctor" runat="server" CssClass="span7" placeholder="Enter your message"></asp:TextBox>
                                </div>
                                <div style="padding-top: 4px;">
                                    <asp:LinkButton ID="btnDoctor" runat="server" Text="Send Message" CssClass="btn btn-danger" OnClick="btnDoctor_Click" OnClientClick="DisableOnSubmit(this);"></asp:LinkButton>

                                    <asp:LinkButton ID="btnDoctorEmail" runat="server" Text="Send Email" CssClass="btn btn-danger" OnClick="btnDoctorEmail_Click" OnClientClick="DisableOnSubmit(this);"></asp:LinkButton>

                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <asp:GridView ID="DoctorGV" runat="server" CssClass="table table-bordered"
                                        AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="txtSelect" runat="server" CssClass="doctor-select" onclick="ChkForDoctor();" />
                                                    <asp:HiddenField ID="txtMobile" runat="server" Value='<%#Eval("MobileNo") %>' />
                                                    <asp:HiddenField ID="txtEmail" runat="server" Value='<%#Eval("MailID") %>' />

                                                    <asp:HiddenField ID="txtFullName" runat="server" Value='<%#Eval("MailID") %>' />
                                                    <asp:HiddenField ID="txtBirthDate" runat="server" Value='<%#Eval("MailID") %>' />
                                                    <asp:HiddenField ID="txtrAddress" runat="server" Value='<%#Eval("MailID") %>' />
                                                    <asp:HiddenField ID="txtRegistrationDate" runat="server" Value='<%#Eval("MailID") %>' />
                                                    <asp:HiddenField ID="txtID" runat="server" Value='<%#Eval("ID") %>' />
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="txtSelectAll" runat="server" onclick="SelectAll('doctor-select', this);" />
                                                </HeaderTemplate>
                                                <ItemStyle CssClass="text-center" />
                                                <HeaderStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FULL NAME">
                                                <ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BIRTH DATE">
                                                <ItemTemplate><%# DbHelper.Configuration.FORMATDATE(Eval("BirthDate").ToString())%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="MOBILE NO" DataField="MobileNo" />
                                            <asp:BoundField HeaderText="MAIL ID" DataField="MailID" />
                                            <asp:TemplateField HeaderText="ADDRESS">
                                                <ItemTemplate><%# Eval("ClinicAddress")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="JOIN DATE">
                                                <ItemTemplate><%# DbHelper.Configuration.FORMATDATE(Eval("JoinDate").ToString())%></ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                                        <PagerStyle CssClass="custome-pagination" />
                                    </asp:GridView>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>


                <ajaxToolkit:TabPanel ID="tb_Manager" runat="server" HeaderText="Manager">
                    <ContentTemplate>
                        <div class="manager-div" style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow manager-msg" style="display: none;">
                                <div class="span7">
                                    <asp:TextBox ID="txtManager" runat="server" CssClass="span7" placeholder="Enter your message"></asp:TextBox>
                                </div>
                                <div style="padding-top: 4px;">
                                    <asp:LinkButton ID="btnManager" runat="server" Text="Send Message" CssClass="btn btn-danger" OnClick="btnManager_Click" OnClientClick="DisableOnSubmit(this);"></asp:LinkButton>
                                    <asp:LinkButton ID="btnManagerEmail" runat="server" Text="Send Email" CssClass="btn btn-danger" OnClick="btnManagerEmail_Click" OnClientClick="DisableOnSubmit(this);"></asp:LinkButton>

                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <asp:GridView ID="ManagerGV" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="txtSelect" runat="server" CssClass="manager-select" onclick="ChkForManager();" />
                                                    <asp:HiddenField ID="txtMobile" runat="server" Value='<%#Eval("CONTACTNO") %>' />
                                                    <asp:HiddenField ID="txtEmail" runat="server" Value='<%#Eval("MailID") %>' />
                                                    <asp:HiddenField ID="txtID" runat="server" Value='<%#Eval("ID") %>' />
                                                    <asp:HiddenField ID="txtFullName" runat="server" Value='<%#Eval("FullName") %>' />
                                                    <asp:HiddenField ID="txtBirthDate" runat="server" Value='<%#Eval("BirthDate") %>' />
                                                    <asp:HiddenField ID="txtrAddress" runat="server" Value='<%#Eval("rAddress") %>' />
                                                    <asp:HiddenField ID="txtRegistrationDate" runat="server" Value='<%#Eval("RegistrationDate") %>' />

                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="txtSelectAll" runat="server" onclick="SelectAll('manager-select', this);" />
                                                </HeaderTemplate>
                                                <ItemStyle CssClass="text-center" />
                                                <HeaderStyle CssClass="text-center" Width="35px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FULL NAME">
                                                <ItemTemplate>
                                                    <%#Eval("FullName").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BIRTH DATE">
                                                <ItemTemplate>
                                                    <%# DbHelper.Configuration.FORMATDATE(Eval("BirthDate").ToString())%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="MOBILE NO" DataField="CONTACTNO" />
                                            <asp:BoundField HeaderText="E-Mail ID" DataField="MailID" />
                                            <asp:TemplateField HeaderText="ADDRESS">
                                                <ItemTemplate>
                                                    <%# Eval("Address")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="REG. DATE">
                                                <ItemTemplate>
                                                    <%# DbHelper.Configuration.FORMATDATE(Eval("JOINDATE").ToString())%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No records found...
                                        </EmptyDataTemplate>
                                        <PagerStyle CssClass="custome-pagination" />
                                    </asp:GridView>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Reception" runat="server" HeaderText="Reception">
                    <ContentTemplate>
                        <div class="reception-div" style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow reception-msg" style="display: none;">
                                <div class="span7">
                                    <asp:TextBox ID="txtReception" runat="server" CssClass="span7" placeholder="Enter your message"></asp:TextBox>
                                </div>
                                <div style="padding-top: 4px;">
                                    <asp:LinkButton ID="btn_Reception" runat="server" Text="Send Message" CssClass="btn btn-danger" OnClick="btn_Reception_Click" OnClientClick="DisableOnSubmit(this);"></asp:LinkButton>
                                    <asp:LinkButton ID="btn_ReceEmail" runat="server" Text="Send Email" CssClass="btn btn-danger" OnClick="btn_ReceEmail_Click" OnClientClick="DisableOnSubmit(this);"></asp:LinkButton>

                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <asp:GridView ID="ReceptionGV" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="txtSelect" runat="server" CssClass="reception-select" onclick="ChkForReception();" />
                                                    <asp:HiddenField ID="txtMobile" runat="server" Value='<%#Eval("CONTACTNO") %>' />
                                                    <asp:HiddenField ID="txtEmail" runat="server" Value='<%#Eval("MailID") %>' />
                                                    <asp:HiddenField ID="txtID" runat="server" Value='<%#Eval("ID") %>' />
                                                    <asp:HiddenField ID="txtFullName" runat="server" Value='<%#Eval("FullName") %>' />
                                                    <asp:HiddenField ID="txtBirthDate" runat="server" Value='<%#Eval("BirthDate") %>' />
                                                    <asp:HiddenField ID="txtrAddress" runat="server" Value='<%#Eval("rAddress") %>' />
                                                    <asp:HiddenField ID="txtRegistrationDate" runat="server" Value='<%#Eval("RegistrationDate") %>' />


                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="txtSelectAll" runat="server" onclick="SelectAll('reception-select', this);" />
                                                </HeaderTemplate>
                                                <ItemStyle CssClass="text-center" />
                                                <HeaderStyle CssClass="text-center" Width="35px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FULL NAME">
                                                <ItemTemplate>
                                                    <%#Eval("FullName").ToString()%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BIRTH DATE">
                                                <ItemTemplate>
                                                    <%# DbHelper.Configuration.FORMATDATE(Eval("BIRTHDATE").ToString())%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="MOBILE NO" DataField="CONTACTNO" />
                                            <asp:BoundField HeaderText="E-Mail ID" DataField="MailID" />
                                            <asp:TemplateField HeaderText="ADDRESS">
                                                <ItemTemplate>
                                                    <%# Eval("ADDRESS")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="REG. DATE">
                                                <ItemTemplate>
                                                    <%# DbHelper.Configuration.FORMATDATE(Eval("JOINDATE").ToString())%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No records found...
                                        </EmptyDataTemplate>
                                        <PagerStyle CssClass="custome-pagination" />
                                    </asp:GridView>
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
</asp:Content>
