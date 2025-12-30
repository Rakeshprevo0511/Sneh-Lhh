<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Site.master" CodeBehind="RevalPopUp.aspx.cs" Inherits="snehrehab.Reports.RevalPopUp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.btn-print{padding: 2px 8px;border-radius: 3px;margin: 0 3px;}
.btn-print:hover{text-decoration: none;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Reval Pop-up :
            </div>
            <div class="pull-right">
                <a href="/Reports/ReportStatus.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
            
             <div style="float:left;">
                    <div class="span3" style="margin:0px;">
                        <asp:DropDownList ID="txtDoctor" runat="server" CssClass="chzn-select span3">
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name"></asp:TextBox>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
                   <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="ReportGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true" >
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="SESSION" DataField="SessionName"/>
                <asp:TemplateField HeaderText="THERAPIST"><ItemTemplate><%#Eval("Therapist").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="LAST REVAL DATE"><ItemTemplate><%# DbHelper.Configuration.FORMATDATE(Eval("AppointmentDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="CURRENT REVAL DATE"><ItemTemplate><%#DbHelper.Configuration.FORMATDATE(Eval("AppointmentFrom").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="POPUP REVAL DATE"><ItemTemplate><%#DbHelper.Configuration.FORMATDATE(Eval("PopUpDate").ToString())%></ItemTemplate></asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="formRow">
        <div class="span6">
            <label class="control-label"></label>
            <div class="control-group">
                <asp:LinkButton runat="server" Text="Add Remark" CssClass="btn btn-danger" ID="btnNo" OnClick="btnNo_Click"></asp:LinkButton>
            </div>
        </div>
    </div>
<div class="modal fade" id="edit_ref_modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: #FFF !important; text-shadow: none; opacity: 1;"><span aria-hidden="true">&times;</span></button>
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
                                <a class="btn btn-danger" data-dismiss="modal">Close</a>
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
