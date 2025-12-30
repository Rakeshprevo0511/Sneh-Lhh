<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Site.master" CodeBehind="DrYesterdayAppointRecord.aspx.cs" Inherits="snehrehab.Member.DrYesterdayAppointRecord" %>

<asp:Content runat="server" ContentPlaceHolderID="head" ID="Content1"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1" ID="Content2">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Dr. Yesterday's Appointment Record
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList runat="server" CssClass="input-medium chzn-select span2" ID="txtStatus">
                        <asp:ListItem Value="-1">All Record</asp:ListItem>
                        <asp:ListItem Value="0">Yes</asp:ListItem>
                        <asp:ListItem Value="1">No</asp:ListItem>
                        <%--<asp:ListItem Value="2">Not Check</asp:ListItem>--%>
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="txtTherapist" runat="server" CssClass="chzn-select input-medium span3"> 
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;margin-top: 4px;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click" >Search</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click" >Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView runat="server" CssClass="table table-bordered table-responsive" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false" ID="yestAppointRecord"
                OnPageIndexChanging="yestAppointRecord_PageIndexChanging" PageSize="30" 
                AllowPaging="true" >
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
           <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
              <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# FORMATDATE(Eval("AddedDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Therapist"><ItemTemplate><%# (Eval("Therapist").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Record"><ItemTemplate><%# (Eval("Yes_No_Value").ToString())%></ItemTemplate></asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
