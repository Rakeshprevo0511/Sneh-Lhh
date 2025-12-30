<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Site.master" CodeBehind="YesterdayAppointments.aspx.cs" Inherits="snehrehab.Member.YesterdayAppointments" %>
<%@ MasterType VirtualPath="~/Member/Site.master"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
               YesterDay's Appointment :
            </div>
        </div>
        <div class="grid-content">
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="AppointmentGV" runat="server" CssClass="table table-bordered" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="AppointmentGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true" >
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="SESSION" DataField="SessionName"/>
                <asp:TemplateField HeaderText="THERAPIST"><ItemTemplate><%#Eval("Therapist").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# FORMATDATE(Eval("AppointmentDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DURATION"><ItemTemplate><%#Eval("Duration").ToString() +" Min"%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="TIME"><ItemTemplate><%#TIMEDURATION(Eval("Duration").ToString(), Eval("AppointmentTime").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="SESSION CHARGES AND STATUS"><ItemTemplate><%#record(Eval("SessionCharge").ToString(), Convert.ToInt32(Eval("AppointmentID")))%></ItemTemplate></asp:TemplateField>


                <%--<asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="80px"/>
                </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
        </div>
    </div>
     <div class="formRow">
        <div class="span6">
            <label class="control-label"></label>
            <div class="control-group">
                <asp:LinkButton runat="server" Text="No" CssClass="btn btn-danger" ID="btnNo" OnClick="btnNo_Click"></asp:LinkButton>
                <asp:LinkButton runat="server" Text="Yes" CssClass="btn btn-info" ID="btnSave" OnClick="btnSave_Click"></asp:LinkButton>
            </div>
        </div>
    </div>
    <div class="modal fade" id="edit_ref_modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">
                        Add Narration</h4>
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
                            <span class="span2"><b>Narration:</b></span>
                            <div class="control-group">
                                <div class="span">
                                    <asp:TextBox ID="txtMessage" runat="server" CssClass="span" TextMode="MultiLine" Rows="6"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>                
                <div class="modal-footer">
                    <asp:UpdatePanel ID="modl" runat="server">
                        <ContentTemplate>
                            <div style="float: right;">
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                                &nbsp;
                                <asp:Button ID="btnSaveRefer" runat="server" class="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" ></asp:Button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function showModal() {
        $("#edit_ref_modal").modal('show');
    }

    $(function () {
        $("#btnNo").click(function () {
            showModal();                   
        });
    });
</script>
</asp:Content>