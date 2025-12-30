<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_SessionChrges" Title="" Codebehind="SessionChrges.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
    .inline-checkbox{display: inline-block;float: left;}
    .inline-checkbox input[type="checkbox"]{padding-right:5px;margin:0px;}
    .inline-checkbox label{height: auto;width: auto !important;float: none;display: inline-block;padding: 5px 10px;line-height: normal;margin: 0px;}
    
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Doctor Session Charges :
            </div>
        </div>
        <div class="grid-content" >
            <div class="formRow" id="btn_update_charges" runat="server">
                <asp:UpdatePanel ID="UpdateSession" runat="server" RenderMode="Inline"><ContentTemplate>
                <asp:DropDownList ID="txtSession" runat="server" CssClass="input-medium" 
                    OnSelectedIndexChanged="txtSession_SelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList>
                </ContentTemplate></asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdateChargeType" runat="server" RenderMode="Inline">
                <ContentTemplate>
                <asp:DropDownList ID="txtChargeType" runat="server" CssClass="input-small" AutoPostBack="true" OnSelectedIndexChanged="txtChargeType_SelectedIndexChanged">
                <asp:ListItem Value="1">Percent</asp:ListItem>
                <asp:ListItem Value="2">Rupees</asp:ListItem>
                </asp:DropDownList>
                </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline"><ContentTemplate>
                <asp:TextBox ID="txtDoctor1" runat="server" placeholder="Doctor 1" Width="150px"></asp:TextBox>
                <asp:TextBox ID="txtDoctor2" runat="server" placeholder="Doctor 2" Width="150px"></asp:TextBox>
                </ContentTemplate></asp:UpdatePanel>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-danger" onclick="btnSearch_Click" OnClientClick="DisableOnSubmit(this);">Add / Update</asp:LinkButton>
            </div>
           <%-- <div class="formRow">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlTime" runat="server" Visible="false">
                            <asp:CheckBox ID="txtTimeWise" runat="server" CssClass="inline-checkbox" Text="  Time wise charges" Enabled="false" />
                            <div class="span2">
                                <table>
                                    <tr>
                                        <th colspan="2">
                                            Doctor : 1
                                        </th>
                                    </tr>
                                    <asp:Repeater ID="txtTime1" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <%#Eval("Duration")%> Min :- &nbsp;&nbsp;
                                                    <asp:HiddenField ID="hidDuration" runat="server" Value='<%#Eval("Duration")%>' />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="span1" Text='<%#Eval("Amount")%>'></asp:TextBox>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                            <div class="span2">
                                <table>
                                    <tr>
                                        <th colspan="2">
                                            Doctor : 2
                                        </th>
                                    </tr>
                                    <asp:Repeater ID="txtTime2" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <%#Eval("Duration")%> Min :- &nbsp;&nbsp;
                                                    <asp:HiddenField ID="hidDuration" runat="server" Value='<%#Eval("Duration")%>' />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="span1" Text='<%#Eval("Amount")%>'></asp:TextBox>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>--%>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="ChargesGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="ChargesGV_PageIndexChanging"
                PageSize="30" AllowPaging="false">
                <PagerStyle CssClass="custome-pagination"></PagerStyle>
                <EmptyDataTemplate>
                    No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="50px" /></asp:TemplateField>
                    <asp:TemplateField HeaderText="SESSION"><ItemTemplate><%#Eval("SessionName").ToString()%></ItemTemplate></asp:TemplateField>
                    <asp:BoundField HeaderText="CHARGE TYPE" DataField="ChargeType" HeaderStyle-Width="120px"><HeaderStyle Width="120px"></HeaderStyle></asp:BoundField>
                    <asp:BoundField HeaderText="DOCTOR-1" DataField="Doctor1" HeaderStyle-Width="100px"><HeaderStyle Width="100px"></HeaderStyle></asp:BoundField>
                    <asp:BoundField HeaderText="DOCTOR-2" DataField="Doctor2" HeaderStyle-Width="100px"><HeaderStyle Width="100px"></HeaderStyle></asp:BoundField>
                    <asp:TemplateField HeaderText="ACTION">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDelete" runat="server" Text="Remove" OnClientClick="return confirm('Are you sure..??');" OnClick="btnDelete_Click" CommandArgument='<%#Eval("SessionID") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle Width="55px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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

