<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupEditPage.aspx.cs" Inherits="TradeService.GroupEditPage" %>

<%@ Register Src="~/Controls/Header.ascx" TagPrefix="uc1" TagName="Header" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        th {
            text-align:right;
            padding:0px 10px 0px 0px;
        }
        .txt{
            float:left;
            border:solid 1px #999999;
        }
        tr {
            height: 36px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="main">
        <uc1:Header runat="server" ID="Header" />
        <table style="margin: 10px auto; background:#EAEAEA; width:100%;">
            <tr>
                <th>名称:</th>
                <td><asp:TextBox runat="server" ID="txtName" class="txt"/></td>
            </tr>
            <tr>
                <th>启用:</th>
                <td><asp:CheckBox runat="server" ID="cbIsUse" Text=" "  style="float:left"/> </td>
            </tr>
            <tr>
                <th>交易服务器:</th>
                <td><asp:TextBox runat="server" ID="txtServerInfo"  class="txt"/></td>
            </tr>
            <tr>
                <th>IP:</th>
                <td><asp:TextBox runat="server" ID="txtIP" class="txt"/></td>
            </tr>
            <tr>
                <th>Port:</th>
                <td><asp:TextBox runat="server" ID="txtPort" TextMode="Number" class="txt"/></td>
            </tr>
            <tr>
                <th>版本号:</th>
                <td><asp:TextBox runat="server" ID="txtVersion"  class="txt"/></td>
            </tr>
            <tr>
                <th>营业部代码:</th>
                <td><asp:TextBox runat="server" ID="txtDepartment" class="txt" /></td>
            </tr>
            <tr>
                <th>登录帐号:</th>
                <td><asp:TextBox runat="server" ID="txtLogAccount" class="txt" /></td>
            </tr>
            <tr>
                <th>交易帐号:</th>
                <td><asp:TextBox runat="server" ID="txtTradeAccount" class="txt" /></td>
            </tr>
            <tr>
                <th>交易密码:</th>
                <td><asp:TextBox TextMode="Password" runat="server" ID="txtTradePsw" class="txt" /></td>
            </tr>
            <tr>
                <th>通讯密码:</th>
                <td><asp:TextBox runat="server" TextMode="Password" ID="txtCommunicatePsw" class="txt" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <%--<div style="margin:10px 0px 0px 300px">
                        
                    </div>--%>
                    <asp:Button ID="btnSave"  OnClick="btnSave_Click" runat="server" Text="保存" style="width:70px; margin: 0 auto; border:solid 1px #999999;" />
                </td>
            </tr>
        </table>

    </div>
    </form>
</body>
</html>
