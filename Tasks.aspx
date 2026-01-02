<%@ Page Language="C#" MasterPageFile="~/SideBar.master"  AutoEventWireup="true" CodeBehind="Tasks.aspx.cs" Inherits="TaskQuest.Tasks" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <title>Tasks Page</title>
    <link rel="stylesheet" href="assets/css/tasks.css">
    <!-- Using Feather Icons for UI elements -->
    <script src="https://unpkg.com/feather-icons"></script>
</asp:Content>

<asp:Content ID="TasksContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tasks-page-container">
        
        <!-- Page Header -->
        <div class="tasks-header-controls">
            <h1 class="tasks-title">Tasks</h1>
            <button class="sort-btn">
                <i data-feather="filter" style="width: 16px; height: 16px;"></i>
                Sort By : All
            </button>
        </div>

        <!-- Projects Columns Wrapper -->
        <div class="projects-columns-wrapper">
            
            <asp:Repeater ID="rptProjects" runat="server" OnItemDataBound="rptProjects_ItemDataBound">
                <ItemTemplate>
                    <div class="project-column">
                        <div class="project-header">
                            <div class="project-header-top">
                                <div class="project-info">
                                    <img src='<%# Eval("ProjectLogo") != DBNull.Value ? Eval("ProjectLogo") : "assets/images/projectTestAvatar.jpg" %>' class="project-icon" alt='<%# Eval("ProjectName") %>' />
                                    <span class="project-name"><%# Eval("ProjectName") %></span>
                                </div>
                                <i data-feather="more-vertical" class="more-options"></i>
                            </div>
                            <div class="progress-container">
                                <div class="progress-track">
                                    <div class="progress-fill" style='<%# "width: " + GetProgressPercentage(Container.DataItem) + "%;" %>'></div>
                                </div>
                                <span class="progress-text"><%# GetProgressPercentage(Container.DataItem) %>%</span>
                            </div>
                            <span class="task-stats"><%# Eval("DoneStatusCount") %> Completed / <%# Convert.ToInt32(Eval("ToDoStatusCount")) + Convert.ToInt32(Eval("InProgressStatusCount")) %> Incomplete</span>
                        </div>

                        <div class="task-list">
                            <asp:Repeater ID="rptTasks" runat="server">
                                <ItemTemplate>
                                    <div class="task-card">
                                        <div class="task-content">
                                            <div class="task-checkbox-wrapper">
                                                <input type="checkbox" class="task-checkbox" <%# Convert.ToInt32(Eval("StatusId")) == 3 ? "checked" : "" %> />
                                                <span class="task-title" style='<%# Convert.ToInt32(Eval("StatusId")) == 3 ? "text-decoration: line-through; color: #bdbdbd;" : "" %>'><%# Eval("Title") %></span>
                                            </div>
                                        </div>
                                        <div class="task-meta">
                                            <span class='<%# "status-badge " + GetStatusClass(Eval("StatusId")) %>'><%# GetStatusText(Eval("StatusId")) %></span>
                                            <span class="task-date"><%# Eval("DueDate", "{0:MMMM d, yyyy}") %></span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <button class="new-task-btn">
                            <i data-feather="plus" style="width: 16px; height: 16px;"></i>
                            New Task
                        </button>
                        <div class="project-footer">
                            <i data-feather="clock" style="width: 12px; height: 12px;"></i>
                            Created: <%# Eval("CreatedAt", "{0:MMM d, yyyy}") %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            
            <asp:Label ID="lblNoProjects" runat="server" Text="No projects found." Visible="false" CssClass="no-projects-msg"></asp:Label>

        </div>
    </div>
    
    <script>
        feather.replace();
    </script>
</asp:Content>