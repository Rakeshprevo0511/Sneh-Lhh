<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_PatientChrges" Title="" Codebehind="PatientChrges.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Registration Charges :
            </div>
        </div>
        <div class="grid-content">
            <div id="update_charge" class="formRow" runat="server">
                <asp:DropDownList ID="txtPatientType" runat="server" CssClass="input-medium">
                </asp:DropDownList>
                <asp:TextBox ID="txtAmount" runat="server" placeholder="Charge Amount" Width="150px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-danger" 
                    onclick="btnSearch_Click">Add / Update</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="ChargesGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="ChargesGV_PageIndexChanging" PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="50px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="REGISTRATION TYPE"><ItemTemplate> <%#Eval("PatientType").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Amount" DataField="ChargeAmt" HeaderStyle-Width="100px"/>
                <asp:TemplateField HeaderText="MODIFY DATE"><ItemTemplate> <%# FORMATDATE(Eval("ModifyDate").ToString())%></ItemTemplate><HeaderStyle Width="120px"/></asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
        </div>
    </div>
</asp:Content>

