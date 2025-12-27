<%@ Page Language="C#" MasterPageFile="~/SideBar.master"  AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="TaskQuest.Projects" %>



<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
<title>Projects Page</title>
    <link rel="stylesheet" href="assets/css/projects.css">
</asp:Content>




<asp:Content ID="ProjectContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="padding:5px 20px;">Projects</h2>
    <div class="projects-container">
    <div class="projects-grid">
        <asp:Repeater ID="rptProjects" runat="server">
            <ItemTemplate>
                <div class="project-card">
                    <div class="project-image">
                        <img src='<%# Eval("ImageUrl") %>' />
                    </div>

                    <div class="project-content">
                        <h3 class="project-title"><%# Eval("Title") %></h3>
                        <p class="project-description"><%# Eval("Description") %></p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
</asp:Content>
