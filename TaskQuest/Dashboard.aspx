<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TaskQuest.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>TaskQuest - داشبورد</title>
    <link rel="stylesheet" href="assets/css/dashboard.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboard-container">
            <div class="welcome-message">
                    <h1><asp:Label ID="lblWelcome" runat="server" Text="کاربر گرامی خوش آمدید"></asp:Label></h1>
                </div>
        </div>
    </form>
</body>
</html>