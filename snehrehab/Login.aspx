<%@ Page Language="C#" AutoEventWireup="true" Inherits="Login" Codebehind="Login.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SnehRehab - Login Panel</title>
    <link href="/css/login.css" rel="stylesheet" type="text/css" />
    <link href="/css/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <link href="/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/css/icon/font-awesome.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id="container_demo">
        <div id="wrapper">
            <div id="login" class="animate form" style="width: 400px; margin: 0 auto;">
                <form id="Form1" class="form-login" runat="server" defaultbutton="btnSubmit">
                <div class="content-login">
                    <center>
                        <div class="inputs">
                            <table>
                                <tr>
                                    <td align="center" colspan="2">
                                        <img src="/images/Sneh-Logo-1_login.png" style="width:289px" alt="" /><br />
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>User Name :</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUsername" runat="server" class="first-input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Password :</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="last-input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Category :</strong>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="txtCategory" runat="server" CssClass="" Style="font-size: 12px;
                                            line-height: 25px; padding: 4px; height: auto;">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Log In" class="btn btn-primary"
                                            OnClick="btnSubmit_Click" OnClientClick="DisableOnSubmit(this);"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" colspan="2">
                                        <asp:PlaceHolder ID="MsgPlace" runat="server"></asp:PlaceHolder>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
                <br />
                </form>
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div style=""></div>
    <script src="/js/jquery.min.js" type="text/javascript"></script>
    <script src="/js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function DisableOnSubmit(btn) {
            try {
                $('body').append('<div style="width: 100%;height: 100%;position: fixed;top: 0;left: 0;z-index: 99999999;"></div>');
            } catch (e) { }
            try { $('#' + btn.id).attr('disabled', 'disabled'); } catch (e) { }
            document.getElementById(btn.id).disabled = 'true';
            document.getElementById(btn.id).text = 'Please Wait...';
        }
   </script>
</body>
</html>
