<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="ReferrenceList.aspx.cs" Inherits="snehrehab.Reports.ReferrenceList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
.appointment-complete{color: #3C8600;}
.appointment-absent{color: #ff0024;}
.appointment-cancel{color: #ff8400;}
.btn-pay{padding: 2px 8px;border-radius: 3px;margin: 0 3px;}
.btn-pay:hover{text-decoration: none;}
.btn-cancel{padding: 2px 5px;border-radius: 3px;margin: 0 3px;background-color: #FCB83B;}
.btn-cancel:hover{text-decoration: none;}
.btn-absent{padding: 2px 5px;border-radius: 3px;margin: 0 3px;}
.btn-absent:hover{text-decoration: none;}
.chzn-select{width:200px}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
    <asp:UpdatePanel ID="new" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="newmsg" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
        <div class="grid-title">
            <div class="pull-left">
                Reference List :
            </div>
            <div class="pull-right">
                <asp:Literal ID="lblAddNew" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="txtReferred" runat="server" CssClass="input-medium chzn-select span2" AutoPostBack="true" OnSelectedIndexChanged="txtreferred_SelectedIndexChanged">
                        <asp:ListItem Value="-1">All</asp:ListItem>
                        <asp:ListItem Value="1" >Reference Doctor</asp:ListItem>
                        <asp:ListItem Value="2">Reference School</asp:ListItem>
                        <asp:ListItem Value="3">Reference Hospital</asp:ListItem>
                        <asp:ListItem Value="4">Reference Teacher</asp:ListItem>
                        <asp:ListItem Value="5">Reference Other</asp:ListItem>
                        <asp:ListItem Value="6">Reference Online</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="txtReferSelected" runat="server" CssClass="chzn-select input-medium span3"> 
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name" CssClass="span2"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;margin-top: 4px;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="ReferenceGV" runat="server" CssClass="table table-bordered table-responsive" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="ReferenceGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true" >
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="CONTACT NO"><ItemTemplate><%#Eval("MobileNo").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ADDRESS"><ItemTemplate><%#Eval("Address").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="EMAIL ID"><ItemTemplate><%#Eval("EmailID").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="WEBSITE"><ItemTemplate><%#Eval("Website").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="REFERENCE NAME"><ItemTemplate><%#Eval("Name").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DATE" Visible=false><ItemTemplate><%# Eval("AddedDate").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION">
                <ItemTemplate>
                <%#GetAction(Eval("ReferredID").ToString(), Eval("Reference_Selected").ToString(), Eval("Name").ToString(), Eval("MobileNo").ToString(), Eval("EmailID").ToString(), Eval("Website").ToString(), Eval("Address").ToString(), Eval("AddedDate").ToString(), Eval("UniqueID").ToString())%>
                </ItemTemplate>                
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="110px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
                                <asp:Button ID="btnSaveRefer" runat="server" class="btn btn-primary" Text="Update" OnClick="btnSaveRefer_Click"></asp:Button>
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
            <asp:HiddenField ID="txtReferedByID" runat="server" />
            <asp:HiddenField ID="txtReferSelectedID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function LoadDetail(ctl,ReferredID, RefSelected, name, mobile, emailid, website, address,date) {
            $('#<%= txtmodname.ClientID%>').val(name);
            $('#<%= txtmodcontact.ClientID %>').val(mobile);
            $('#<%= txtmodemailid.ClientID %>').val(emailid);
            $('#<%= txtmodwebsite.ClientID %>').val(website);
            $('#<%= txtmodaddress.ClientID %>').val(address);
            $('#<%= txtmodAddedDate.ClientID %>').val(date);
            $('#<%= txtReferedByID.ClientID %>').val(ReferredID);
            $('#<%= txtReferSelectedID.ClientID %>').val(RefSelected);
            
            $('#edit_ref_modal').modal('show').off('hidden.bs.modal')
        }
        function close_modal() {
            $('#edit_ref_modal').modal('hide').on('hidden.bs.modal');
        }
    </script>
</asp:Content>
