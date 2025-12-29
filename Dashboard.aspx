
<%@ Page Language="C#" MasterPageFile="~/SideBar.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TaskQuest.Dashboard" %>


<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
<title>Dashboard Page</title>
</asp:Content>


<asp:Content ID="DashboardContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-container">
        <div class="coming-soon-wrapper">
            <h1>coming soon</h1>
        </div>
    </div>
    <style>
        .coming-soon-wrapper {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 80vh; /* Adjust height to center vertically */
        }
        .coming-soon-wrapper h1 {
            font-family: 'Running', sans-serif;
            font-size: 64px; /* Adjust size as needed */
            color: #333;
        }
    </style>
</asp:Content>
