<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="TradeService.Manage" %>

<%@ Register Src="~/Controls/Header.ascx" TagPrefix="uc1" TagName="Header" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="script/zDialog.js"></script>
    <script src="script/zDrag.js"></script>
    <script>
        function validate() {
            var p0 = document.getElementById("txtPsw");
            var p1 = document.getElementById("txtPsw");
            if (p0.value == p1.value) {
                return true;
            }
            else {
                alert("两次密码输入不一致！");
                return false;
            }
        }
        function showEdit(){
            var diag = new Dialog();
            diag.Title = "带有说明栏的新窗口";
            diag.Width = 900;
            diag.Height = 400;
            diag.URL = "GroupEditPage.aspx";
            diag.AutoClose = -1;
            diag.show();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div style="margin:0px auto; width:1100px;">
            <uc1:Header runat="server" ID="Header" />
            <table class="ts" style="text-align:left">
                <asp:Repeater ID="rptMain" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th style="width: 40px">名称</th>
                            <th style="width: 40px">启用</th>
                            <th style="width: 300px">交易服务器</th>
                            <th style="width: 120px">IP</th>
                            <th style="width: 60px">Port</th>
                            <th style="width: 60px">版本号</th>
                            <th style="width: 100px">营业部代码</th>
                            <th style="width: 100px">登录帐号</th>
                            <th style="width: 100px">交易帐号</th>
                            <th style="width: 60px">操作</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("名称") %>
                            </td>
                            <td>
                                <%#Eval("启用") %>
                            </td>
                            <td>
                                <%#Eval("交易服务器") %>
                            </td>
                            <td>
                                <%#Eval("IP") %>
                            </td>
                            <td>
                                <%#Eval("Port") %>
                            </td>
                            <td>
                                <%#Eval("版本号") %>
                            </td>
                            <td>
                                <%#Eval("营业部代码") %>
                            </td>
                            <td>
                                <%#Eval("登录帐号") %>
                            </td>
                            <td>
                                <%#Eval("交易帐号") %>
                            </td>
                            <td>
                                <asp:Button ID="btnEdit" runat="server" Text ="编辑" OnClick="BtnEdit_Click" ToolTip='<%#Bind("名称") %>'  />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </form>
</body>
</html>
