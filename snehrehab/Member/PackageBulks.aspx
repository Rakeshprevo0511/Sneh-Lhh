<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="PackageBulks.aspx.cs" Inherits="snehrehab.Member.PackageBulks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function BookingDetail(id) {
            $('#booking-detail div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Loading Please Wait...</span>');
            $('#booking-detail').modal('show');
            $.ajax({
                type: "POST", url: "/Snehrehab.asmx/BulkPackageDetail", data: "{'id':'" + id + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (response) {
                    if (response.d != null) {
                        $('#booking-detail div.modal-body').html(response.d);
                    } else {
                        $('#booking-detail div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Error: Unable to process, Please try again.</span>');
                    }
                },
                error: function (msg) {
                    $('#booking-detail div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Error: ' + msg.statusText + '</span>');
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Bulk Booking List :
            </div>
            <div class="pull-right">
                <asp:Literal ID="lblAddNew" runat="server"></asp:Literal>                
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name"></asp:TextBox>
                <asp:DropDownList ID="txtPayMode" runat="server" Width="100px" Height="24px">
                    <asp:ListItem Value="0">Any Mode</asp:ListItem>
                    <asp:ListItem Value="1">Cash</asp:ListItem>
                    <asp:ListItem Value="2">Credit</asp:ListItem>
                    <asp:ListItem Value="3">Cheque</asp:ListItem>
                    <asp:ListItem Value="4">Online</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <div class="alert alert-info">
            <strong>Total Booking Amount : 
                <asp:Label ID="lblTotal" runat="server" ></asp:Label>/- INR</strong>
            </div>
            <asp:GridView ID="BookingGV" runat="server" CssClass="table table-bordered" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="BookingGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="TELEPHONE" DataField="TelephoneNo"/>
                <asp:BoundField HeaderText="CITY" DataField="CityName"/>
                <asp:BoundField HeaderText="PATIENT TYPE" DataField="PatientType"/> 
                <asp:TemplateField HeaderText="AMOUNT"><ItemTemplate><%#Eval("Amount")%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="PAID DATE"><ItemTemplate><%# FORMATDATE(Eval("PaidDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="BOOKING DATE"><ItemTemplate><%# FORMATDATETIME(Eval("AddedDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="CARRY FWD"><ItemTemplate><%# TOTALBALANCE(Eval("TotalBalance").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                    <%# DELETELINK(Eval("UniqueID").ToString(), Eval("CanDelete").ToString(), Eval("UniqueID").ToString(), Eval("CanDelete1").ToString())%>
                    <a href="javascript:;" onclick='BookingDetail("<%#Eval("UniqueID") %>");'>View</a>
                    <%--<a href='/Member/PackageBulk.aspx?record=<%# Eval("UniqueID") %>'>Edit</a> --%>
                    <%# GetEdit(Eval("UniqueID").ToString()) %>
                    <span style="display:none;" class="apt-rmk"><%#Eval("Narration")%></span>
                        <br />
                  
                    <%# CHECKBULKUSAGELINK(Eval("BulkID").ToString(), Eval("CanDelete").ToString()) %>

                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="100px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="modal fade" id="booking-detail" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title" style="margin: 0px;">Booking Detail</h5>
                </div>
                <div class="modal-body">                     
                </div>
            </div>
        </div>
    </div>

     <script type="text/javascript">
    $(function () {
        $('span.apt-rmk').each(function () {
            if ($(this).html().length > 0) {
                $('<tr><td></td><td colspan="8"><i>' + $(this).html() + '</i></td></tr>').insertAfter($(this).closest('tr'));
            }
        });
    });
</script>
</asp:Content>
