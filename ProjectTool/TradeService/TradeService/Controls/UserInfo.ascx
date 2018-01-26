<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.ascx.cs" Inherits="TradeService.Controls.UserInfo" %>
<script type="text/javascript">
    function validate()
    {
        if (txtPsw.value != txtPswAgain.value) {
            alert("两次输入的密码不一致！");
            return false;
        }
    }
</script>
<table style="width:100%;height:150px;">
    <tr>
        <td style="width:45%;">
            <label style="float:right;">用户名：</label>
        </td>
        <td>
            <input type="text" runat="server" id="txtUserName" style="float:left;border:solid 1px #999999;"/>
        </td>
    </tr>
    <tr>
        <td>
            <label style="float:right;">密码：</label>
        </td>
        <td>
            <input type="password" runat="server" id="txtPsw" style="float:left;border:solid 1px #999999;" />
        </td>
    </tr>
    <tr>
        <td>
            <label style="float:right;">确认密码：</label>
        </td>
        <td>
            <input type="password" runat="server" id="txtPswAgain" style="float:left;border:solid 1px #999999;"/>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button Text="确认" OnClientClick="validate()" OnClick="btnSubmit_Click" ID="btnSubmit" runat="server" style="width:70px; margin:0 auto;border:solid 1px #999999;"/>
        </td>
    </tr>
</table>
