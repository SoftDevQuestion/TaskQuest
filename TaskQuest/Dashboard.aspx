
<%@ Page Language="C#" MasterPageFile="~/SideBar.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TaskQuest.Dashboard" %>


<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
<title>Dashboard Page</title>
</asp:Content>


<asp:Content ID="DashboardContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="dashboard-container">
            <div class="welcome-message">
                    <h1><asp:Label ID="lblWelcome" runat="server" Text="کاربر گرامی خوش آمدید"></asp:Label></h1>
                </div>
        </div>
    </asp:Content>
