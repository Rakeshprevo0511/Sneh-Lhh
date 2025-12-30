<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="ViewBulkPackageDetails.aspx.cs" Inherits="snehrehab.Member.ViewBulkPackageDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:HiddenField ID="hfBulkBookingID" runat="server" Value="0" />
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
               Bulk Package Details List :
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
                ₹ <%# Eval("TotalPackageAmount") ?? "0" %>
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

    <HeaderStyle Width="180px" CssClass="text-center" />
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

     

    </Columns>

</asp:GridView>

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

