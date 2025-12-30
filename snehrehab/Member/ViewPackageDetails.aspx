<%@ Page Language="C#" 
    MasterPageFile="~/Member/Site.master"
    AutoEventWireup="true"
    Inherits="snehrehab.Member.ViewPackageDetails"
    Title="View Package Details"
    Codebehind="ViewPackageDetails.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function ViewPackageDetail(id) {
            $('#package-detail div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Loading Please Wait...</span>');
            $('#package-detail').modal('show');
            $.ajax({
                type: "POST", 
                url: "/Snehrehab.asmx/GetPackageDetail", 
                data: "{'id':'" + id + "'}",
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                success: function (response) {
                    if (response.d != null) {
                        $('#package-detail div.modal-body').html(response.d);
                    } else {
                        $('#package-detail div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Error: Unable to process, Please try again.</span>');
                    }
                },
                error: function (msg) {
                    $('#package-detail div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Error: ' + msg.statusText + '</span>');
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Package Details List :
            </div>
            <div class="pull-right">
                <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-primary" OnClick="btnBack_Click">
                  <i class="fa fa-arrow-left"></i> Back to Packages
                </asp:LinkButton>
                 <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" Style="margin-left:10px;" OnClick="btnExport_Click">Export</asp:LinkButton>
            </div>
             
  
        </div>

       <div class="grid-content">
    <div class="formRow">
       
    </div>
    <div class="clearfix"></div>
    
    <div class="panel panel-success">
       <asp:HiddenField ID="hfBookingID" runat="server" Value="0" />
        <div class="panel-body">
         <asp:GridView ID="BookingGV" runat="server"
    CssClass="table table-bordered table-striped"
    AutoGenerateColumns="false" AllowPaging="false">

    <EmptyDataTemplate>No session usage found...</EmptyDataTemplate>

    <Columns>

        <asp:TemplateField HeaderText="SR NO">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
            <HeaderStyle Width="60px" CssClass="text-center" />
            <ItemStyle CssClass="text-center" />
        </asp:TemplateField>

       

        <asp:TemplateField HeaderText="APPOINTMENT DATE">
            <ItemTemplate>
                <%# Eval("AppointmentDate") != DBNull.Value 
                    ? Convert.ToDateTime(Eval("AppointmentDate")).ToString("dd-MMM-yyyy") 
                    : "-" %>
            </ItemTemplate>
            <HeaderStyle Width="140px" CssClass="text-center"/>
            <ItemStyle CssClass="text-center" />
        </asp:TemplateField>

       <asp:TemplateField HeaderText="TIME">
    <ItemTemplate>
        <%# Eval("AppointmentTime") != DBNull.Value 
            ? Convert.ToDateTime(Eval("AppointmentTime")).ToString("hh:mm tt")
            : "-" %>
    </ItemTemplate>
    <HeaderStyle Width="100px" CssClass="text-center" />
    <ItemStyle CssClass="text-center" />
</asp:TemplateField>
        <asp:TemplateField HeaderText="Pakage Amount">
            <ItemTemplate>
                ₹ <%# Eval("PackageAmount") ?? "0" %>
            </ItemTemplate>
            <HeaderStyle Width="100px" CssClass="text-right" />
            <ItemStyle CssClass="text-right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Appointment Charge">
            <ItemTemplate>
                ₹ <%# Eval("AppointmentCharge") ?? "0" %>
            </ItemTemplate>
            <HeaderStyle Width="100px" CssClass="text-right" />
            <ItemStyle CssClass="text-right" />
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="REMAINING BALANCE">
            <ItemTemplate>
                <strong style='color:<%# Convert.ToDecimal(Eval("RemainingBalance")) > 0 ? "green" : "red" %>'>
                    ₹ <%# Convert.ToDecimal(Eval("RemainingBalance")).ToString("0.00") %>
                </strong>
            </ItemTemplate>
            <HeaderStyle Width="150px" CssClass="text-right" />
            <ItemStyle CssClass="text-right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Entry DATE">
            <ItemTemplate>
                <%# Eval("AddedDate") != DBNull.Value 
                    ? Convert.ToDateTime(Eval("AddedDate")).ToString("dd-MMM-yyyy") 
                    : "-" %>
            </ItemTemplate>
            <HeaderStyle Width="140px" CssClass="text-center"/>
            <ItemStyle CssClass="text-center" />
        </asp:TemplateField>
      <asp:TemplateField HeaderText="ADDED DATE">
    <ItemTemplate>
      <%# 
    Eval("EntryDate") != DBNull.Value
    ? Convert.ToDateTime(Eval("EntryDate")).ToString("dd-MMM-yyyy hh:mm tt")
    : "-"
%>
    </ItemTemplate>

    <HeaderStyle Width="150px" CssClass="text-center" />
    <ItemStyle CssClass="text-center" />
</asp:TemplateField>
        <asp:TemplateField HeaderText="Added By">
            <ItemTemplate>
                <%# Eval("AddedByName") ?? "-" %>
            </ItemTemplate>
        </asp:TemplateField>
  <asp:TemplateField HeaderText="Modify By">
            <ItemTemplate>
                <%# Eval("ModifyByName") ?? "-" %>
            <span style="display:none;" class="apt-rmk"><%#Eval("Narration")%></span>
            </ItemTemplate>
        </asp:TemplateField>
       <%-- <asp:TemplateField HeaderText="NARRATION">
            <ItemTemplate>
                <%# Eval("Narration") ?? "-" %>
                
            </ItemTemplate>
        </asp:TemplateField>--%>

    </Columns>

</asp:GridView>

        </div>
    </div>
</div>

    </div>

    <div class="modal fade" id="package-detail" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h5 class="modal-title" style="margin: 0px;">Package Detail</h5>
                </div>
                <div class="modal-body">                     
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function () {
            // Show package description below row if available
            $('span.pkg-desc').each(function () {
                if ($(this).html().length > 0) {
                    $('<tr><td></td><td colspan="8"><i class="text-muted">' + $(this).html() + '</i></td></tr>').insertAfter($(this).closest('tr'));
                }
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('span.apt-rmk').each(function () {
                if ($(this).html().length > 0) {
                    $('<tr><td></td><td colspan="8"><i>' + $(this).html() + '</i></td></tr>').insertAfter($(this).closest('tr'));
                }
            });
        });
    </script>
    <style>
        .label {
            display: inline-block;
            padding: 3px 8px;
            font-size: 11px;
            font-weight: bold;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            border-radius: 3px;
        }
        .label-success {
            background-color: #5cb85c;
        }
        .label-danger {
            background-color: #d9534f;
        }
        .btn-sm {
            padding: 3px 8px;
            font-size: 12px;
            line-height: 1.5;
            border-radius: 3px;
        }
        .btn-info {
            color: #fff;
            background-color: #5bc0de;
            border-color: #46b8da;
        }
        .btn-info:hover {
            background-color: #31b0d5;
            border-color: #269abc;
        }
    </style>
</asp:Content>


