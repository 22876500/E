<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="TradeService.UserEdit" %>

<%@ Register Src="~/Controls/UserInfo.ascx" TagPrefix="uc1" TagName="UserInfo" %>
<%@ Register Src="~/Controls/Header.ascx" TagPrefix="uc1" TagName="Header" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="main">
        <uc1:Header runat="server" ID="Header" />
        <uc1:UserInfo runat="server" ID="UserInfo" />
    </div>
    </form>
</body>
</html>
