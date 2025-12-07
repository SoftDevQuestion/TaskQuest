<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestEncoding.aspx.cs" Inherits="TaskQuest.TestEncoding" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>تست انکودینگ</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>تست متن فارسی</h1>
            <p>این یک متن تست برای بررسی انکودینگ فارسی است.</p>
            <asp:Label ID="Label1" runat="server" Text="متن فارسی در Label"></asp:Label>
            <br />
            <asp:Button ID="Button1" runat="server" Text="دکمه تست" />
        </div>
    </form>
</body>
</html>