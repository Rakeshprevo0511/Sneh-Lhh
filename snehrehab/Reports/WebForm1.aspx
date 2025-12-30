<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="snehrehab.Reports.WebForm1" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    function LoadAccount(a, b) {
        $('#myModal div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Loading Please Wait...</span>');
        $.ajax({
        type: "POST", url: "/Snehrehab.asmx/MFusion Comparision:Non Uniform FARonthlyAccount", data: "{'d':'" + a + "','t':'" + b + "'}",
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function(response) {
                $('#myModal div.modal-body').html(response.d);
            },
            error: function(msg) {
                $('#myModal div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Error: ' + msg.statusText + '</span>');
            }
        });
    }
    function DoctorAccount(a, b) {
        $('#myModal div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Loading Please Wait...</span>');
        $.ajax({
            type: "POST", url: "/Snehrehab.asmx/MonthlyAccount_Doctor", data: "{'d':'" + a + "','id':'" + b + "'}",
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function(response) {
                $('#myModal div.modal-body').html(response.d);
            },
            error: function(msg) {
                $('#myModal div.modal-body').html('<span style="padding: 10px;display: block;background: #F4F4F4;">Error: ' + msg.statusText + '</span>');
            }
        });
    }
</script>
    
    
    
    
    
    
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
              Graph :</div>
            <div class="pull-right">
                <a href="/Reports/" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div style="float:left;">
                <div class="span2" style="width: 100px;margin: 0px;">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                </div>
                <div class="span2" style="width: 100px;margin-left: 15px;margin-right:15px;">
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
                </div>
                </div>
                <div style="float:left;margin-top: 4px;">
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" 
                        OnClick="btnExport_Click" Visible="False">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <div style="white-space: nowrap;overflow-x:auto ;" >
            <%--<asp:GridView ID="ReportGV" runat="server" onrowdatabound="ReportGV_RowDataBound" 
                    CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"  OnPageIndexChanging="ReportGV_PageIndexChanging"
                PageSize="31" AllowPaging="true"  ShowFooter="true">
              
<PagerStyle CssClass="custome-pagination"></PagerStyle>
              
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
             
            </asp:GridView>--%>
            <%--style="width: 40px; height: 40px; overflow: hidden; display: inline;"--%>
            
            <table>
            <tr>
            <td>
            <asp:Chart ID="Chart1"  runat="server" 
                    BackGradientStyle="Center"  >
                    <Titles>
                        <asp:Title Font="Microsoft Sans Serif, 9.75pt, style=Bold" Name="Title1" 
                            Text="New Registration List">
                        </asp:Title>
                    </Titles>
                    <Series>
                        <asp:Series Name="NO of Patient" CustomProperties="PointWidth=0.4">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                            <AxisY Title="New Registrations">
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
           </asp:Chart>
           </td>
            <td>
            <asp:Chart ID="Chart2"  runat="server" Palette="EarthTones"  >
                    <Titles>
                        <asp:Title Font="Microsoft Sans Serif, 9.75pt, style=Bold" Name="Title1" 
                            Text="Total No. of Appointments">
                        </asp:Title>
                    </Titles>
                    <Series>
                        <asp:Series Name="NO of Appointments" CustomProperties="PointWidth=0.4">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                            <AxisY Title="Total No. of Appointments">
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
           </asp:Chart>
            </td>
            <td>
            <asp:Chart ID="Chart3"  runat="server"  >
                    <Titles>
                        <asp:Title Font="Microsoft Sans Serif, 9.75pt, style=Bold" Name="Title1" 
                            Text="Session Split Up">
                        </asp:Title>
                    </Titles>
                    <Series>
                        <asp:Series ChartArea="ChartArea1"  ChartType="StackedColumn" Name="Pending" 
                            CustomProperties="PointWidth=0.8" Label="Pending"> </asp:Series>
                        <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" Name="Completed" 
                            CustomProperties="PointWidth=0.8" Label="Completed"> </asp:Series>
                        <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" Name="Absent" 
                            CustomProperties="PointWidth=0.8" Label="Absent"> </asp:Series>
                        <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" Name="Cancelled" 
                            CustomProperties="PointWidth=0.8" Label="Cancelled"> </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                            <AxisY Title="Session Split Up">
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
           </asp:Chart>
            </td>
           <td>
           
           
           <asp:Chart ID="Chart4" runat="server">
                    <Series>
                        <asp:Series Name="Series1">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
           
           
           </td>
                
            </tr>
            </table>
            </div>
        </div>
    </div>
<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title" style="margin: 0px;">
                      Graph</h5>
                </div>
                <div class="modal-body">
                     
                </div>
                <div class="modal-footer">
                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>
