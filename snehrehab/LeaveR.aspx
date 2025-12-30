<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveR.aspx.cs" Inherits="snehrehab.LeaveR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SnehReha :: Leave Reject</title> 
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnlInvalid" runat="server" Visible="false">
        <div style="width: 100%; max-width: 500px; margin: 0 auto; border: 3px solid #c80000;
            font-family: Trebuchet MS; color: #636363; font-size: 15px; margin-top: 10%;
            padding: 20px;">
            <div style="text-align: center; margin-top: -70px;">
                <img src="/images/leave_stop.png" alt="" style="background: #FFF;padding-left: 5px;padding-right: 5px;" />
            </div>
            <br />
            <p style="font-size: 3em; margin: 0px; padding: 0px; margin-bottom: 40px; color: #c80000;text-align: center;">
                Failed</p>
            <p style="text-align: center; line-height: 28px;">
                Your request is invalid.
                <br />
                Please check link in your email account.
            </p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
        <div style="width: 100%; max-width: 500px; margin: 0 auto; border: 3px solid #01c503;
            font-family: Trebuchet MS; color: #636363; font-size: 15px; margin-top: 10%;
            padding: 20px;">
            <div style="text-align: center; margin-top: -70px;">
                <img src="/images/leave_rejected.png" alt="" style="background: #FFF;padding-left: 5px;padding-right: 5px;" />
            </div>
            <br />
            <p style="font-size: 3em; margin: 0px; padding: 0px; margin-bottom: 40px; color: #01c503;text-align: center;">
                Success</p>
            <p style="text-align: center; line-height: 28px;">
                Leave application for <b><%=fullName%></b>
                <br />
                Rejected Successfully.
            </p>
        </div>
    </asp:Panel> 
    <asp:Panel ID="pnlFailed" runat="server" Visible="false">
        <div style="width: 100%; max-width: 500px; margin: 0 auto; border: 3px solid #c80000;
            font-family: Trebuchet MS; color: #636363; font-size: 15px; margin-top: 10%;
            padding: 20px;">
            <div style="text-align: center; margin-top: -70px;">
                <img src="/images/leave_stop.png" alt="" style="background: #FFF;padding-left: 5px;padding-right: 5px;" />
            </div>
            <br />
            <p style="font-size: 3em; margin: 0px; padding: 0px; margin-bottom: 40px; color: #c80000;text-align: center;">
                Failed</p>
            <p style="text-align: center; line-height: 28px;">
                Unable to process your request.
                <br />
                Please try again.
            </p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlPlaced" runat="server" Visible="false">
        <div style="width: 100%; max-width: 500px; margin: 0 auto; border: 3px solid #c80000;
            font-family: Trebuchet MS; color: #636363; font-size: 15px; margin-top: 10%;
            padding: 20px;">
            <div style="text-align: center; margin-top: -70px;">
                <img src="/images/leave_stop.png" alt="" style="background: #FFF;padding-left: 5px;padding-right: 5px;" />
            </div>
            <br />
            <p style="font-size: 3em; margin: 0px; padding: 0px; margin-bottom: 40px; color: #c80000;text-align: center;">
                Failed</p>
            <p style="text-align: center; line-height: 28px;">
                Leave application for <b><%=fullName%></b>
                <br />
                is already completed.
            </p>
        </div>
    </asp:Panel>
    </form>
</body>
</html>
