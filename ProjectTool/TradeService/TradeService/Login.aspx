<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TradeService.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Style/c1.css" rel="stylesheet" />
    <style>
        body {
            background:#eeeeee;
        }
        th {
            text-align:right;
            font-weight:normal;
        }
        .txt {
            border: 1px solid silver;
            float:left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="width:100%;">
    <div id="main" style="width:320px;">
        <table style="width:100%; height:200px; ">
            <tr>
                <th style="width:102px;">
                    <label>用户名：</label>
                </th>
                <td>
                    <input type="text" runat="server" id="txtUserName" class="txt"/>
                </td>
            </tr>
            <tr>
                <th>
                    <label>密码：</label>
                </th>
                <td>
                    <input type="password"  runat="server" id="txtPsw"  class="txt"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button OnClick="btnLogon_Click" ID="btnSubmit" runat="server" Text="登录" style="margin:10px auto 10px auto; width: 70px;"/>
                </td>
            </tr>
        </table>
        
    </div>
    </form>
</body>
</html>
