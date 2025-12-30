<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="AppChngeRequest.aspx.cs" Inherits="snehrehab.Member.AppChngeRequest" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.request-complete{color: #3C8600;}
.request-reject{color: #ff0024;}
.btn-change{padding: 2px 8px;border-radius: 3px;margin: 0 3px;}
.btn-change:hover{text-decoration: none;}
.btn-reject{padding: 2px 5px;border-radius: 3px;margin: 0 3px;background-color: #FCB83B;}
.btn-reject:hover{text-decoration: none;}
.btn-delete{padding: 2px 5px;border-radius: 3px;margin: 0 3px;}
.btn-delete:hover{text-decoration: none;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Doctor Change Request :
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:DropDownList ID="txtStatus" runat="server" CssClass="input-medium">
                    <asp:ListItem Value="0">Pending</asp:ListItem>
                    <asp:ListItem Value="1">Completed</asp:ListItem>
                    <asp:ListItem Value="2">Rejected</asp:ListItem>
                    <asp:ListItem Value="-1">All Records</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="ReportGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true" onrowdatabound="ReportGV_RowDataBound">
                <PagerStyle CssClass="custome-pagination"></PagerStyle>
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate> <%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="REQUEST FROM"><ItemTemplate> <%# Eval("DoctorFrom").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="PATIENT NAME"><ItemTemplate> <%# Eval("PatientFullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="SESSION"><ItemTemplate> <%# Eval("SessionName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DURATION"><ItemTemplate> <%# Eval("Duration").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="REMARK"><ItemTemplate> <%# Eval("Remarks").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DATE"><ItemTemplate> <%# FORMATDATE(Eval("AddedDate").ToString())%></ItemTemplate><HeaderStyle Width="85px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                    <%#GETACTION(Eval("UniqueID").ToString(), Eval("RequestStatus").ToString())%>
                    <asp:HiddenField ID="txtRequestStatus" runat="server" Value='<%#Eval("RequestStatus") %>'/>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="70px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
