<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_DoctorChrges" Title="" Codebehind="DoctorChrges.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Doctor Custom Charges :</div>
            <div class="pull-right">
                
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div style="float:left;">
                <div class="span3" style="margin:0px;">
                <asp:UpdatePanel ID="UpdateDoctor" runat="server"><ContentTemplate>
                <asp:DropDownList ID="txtDoctor" runat="server" CssClass="chzn-select span3" AutoPostBack="true" onselectedindexchanged="txtDoctor_SelectedIndexChanged"></asp:DropDownList>
                </ContentTemplate></asp:UpdatePanel>
                </div>
                <div class="span3" style="margin:0px;">
                <asp:UpdatePanel ID="UpdatePSession" runat="server"><ContentTemplate>
                <asp:DropDownList ID="txtSession" runat="server" CssClass="chzn-select span3"></asp:DropDownList>
                </ContentTemplate></asp:UpdatePanel>
                </div>
                <div class="span2" style="margin:0px;">
                <asp:DropDownList ID="txtType" runat="server" CssClass="chzn-select span2">
                <asp:ListItem Value="1">Percent (%)</asp:ListItem>
                <asp:ListItem Value="2">Rupees (INR)</asp:ListItem>
                </asp:DropDownList>
                </div>
                <div class="span2" style="width: 100px;margin-left: 15px;margin-right:15px;">
                <asp:TextBox ID="txtAmount" runat="server" CssClass="span2" placeholder="Charges" Width="100px"></asp:TextBox>
                </div>
                </div>
                <div style="float:left;margin-top: 4px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-danger" OnClick="btnSubmit_Click" OnClientClick="DisableOnSubmit(this);">Add / Update</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <asp:UpdatePanel ID="UpdateData" runat="server"><ContentTemplate>
            <asp:GridView ID="ChargesGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" AllowPaging="false">
                <PagerStyle CssClass="custome-pagination"></PagerStyle>
                <EmptyDataTemplate>
                    No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="50px" /></asp:TemplateField>
                    <asp:TemplateField HeaderText="NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="SESSION"><ItemTemplate><%#Eval("SessionName").ToString()%></ItemTemplate></asp:TemplateField>
                    <asp:BoundField HeaderText="CHARGE TYPE" DataField="ChargeType" HeaderStyle-Width="120px"><HeaderStyle Width="120px"></HeaderStyle></asp:BoundField>
                    <asp:BoundField HeaderText="CHARGE" DataField="ChargeAmt" HeaderStyle-Width="100px"><HeaderStyle Width="100px"></HeaderStyle></asp:BoundField>
                    <asp:TemplateField HeaderText="ACTION">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDelete" runat="server" Text="Remove" OnClientClick="return confirm('Are you sure..??');" OnClick="btnDelete_Click" CommandArgument='<%#Eval("ChargeID") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle Width="55px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </ContentTemplate></asp:UpdatePanel>
            <br />
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
</asp:Content>

