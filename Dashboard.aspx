<%@ Page Language="C#" MasterPageFile="~/SideBar.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TaskQuest.Dashboard" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <title>Dashboard | TaskQuest</title>
    <link rel="stylesheet" href="assets/css/dashboard.css" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</asp:Content>

<asp:Content ID="DashboardContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-container">
        
        <!-- Left Column (Main) -->
        <div class="main-column">
            
            <!-- Top Header Banner -->
            <div class="card header-card">
                <div class="header-content">
                    <h1 class="header-title">Time for Quest</h1>
                    <p class="header-subtitle">let's manage the teams and tasks</p>
                    <a href="Projects.aspx" class="btn-primary">Create Your Project</a>
                </div>
                <img src="assets/img/hero/hero-5/hero-img.svg" alt="Time for Quest" class="header-image" style="max-height: 200px;" />
            </div>

            <!-- Recent Projects -->
            <div class="projects-row">
                <asp:Repeater ID="rptRecentProjects" runat="server" OnItemDataBound="rptRecentProjects_ItemDataBound">
                    <ItemTemplate>
                        <div class="card project-card">
                            <div class="project-header">
                                <span class="project-date"><%# GetDateString(Eval("UpdatedAt")) %></span>
                                <h3 class="project-title"><%# Eval("ProjectName") %></h3>
                                <p class="project-category"><%# Eval("Description") %></p>
                            </div>
                            
                            <div class="progress-wrapper">
                                <div class="progress-bar-bg">
                                    <div class="progress-bar-fill" style='<%# "width: " + Eval("Progress") + "%; background-color: " + Eval("Color") %>'></div>
                                </div>
                                <div class="progress-info">
                                    <span>Progress</span>
                                    <span><%# Eval("Progress") %>%</span>
                                </div>
                            </div>

                            <div class="project-footer">
                                <div class="left-section" style="display: flex; align-items: center;">
                                    <div class="team-avatars">
                                        <asp:Repeater ID="rptTeamMembers" runat="server">
                                            <ItemTemplate>
                                                <img src='<%# Eval("AvatarPath") %>' class="team-avatar" title='<%# Eval("Username") %>' />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <button type="button" class="btn-add-mini" style='<%# "background-color: " + Eval("Color") %>'>+</button>
                                </div>
                                <div class="right-section">
                                    <span class="days-left" style='<%# "background-color: " + Eval("DaysLeftColor") + "; color: " + Eval("DaysLeftTextColor") + "; padding: 4px 12px; border-radius: 12px; font-size: 12px;" %>'>
                                        <%# Eval("DaysLeftText") %>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- Statistics Chart -->
            <div class="card stats-card">
                <div class="stats-header">
                    <h3 class="stats-title">Tasks Completed</h3>
                </div>
                <div style="height: 250px; width: 100%;">
                    <canvas id="tasksChart"></canvas>
                </div>
            </div>

        </div>

        <!-- Right Column (Sidebar) -->
        <div class="right-sidebar">
            
            <!-- Assistant Card -->
            <div class="card assistant-card">
                <h4 class="assistant-title">Quin</h4>
                <p class="assistant-subtitle">Your Professional Guide</p>
                
                <img src="assets/img/about/about-4/about-img.svg" alt="Quest Assistant" class="assistant-avatar" />
                
                <h5 class="assistant-desc">Let's Get to know us better</h5>
                <p class="assistant-text">Here is a full document for your guidance</p>
                
                <button class="btn-primary">Download</button>
            </div>

            <!-- Calendar Card -->
            <div class="card calendar-card">
                <div class="calendar-header">
                    <asp:Label ID="lblCalendarMonth" runat="server"></asp:Label>
                </div>
                <div class="calendar-grid">
                    <div class="calendar-day-name">Mon</div>
                    <div class="calendar-day-name">Tue</div>
                    <div class="calendar-day-name">Wed</div>
                    <div class="calendar-day-name">Thu</div>
                    <div class="calendar-day-name">Fri</div>
                    <div class="calendar-day-name">Sat</div>
                    <div class="calendar-day-name">Sun</div>
                    
                    <asp:Literal ID="litCalendarDays" runat="server"></asp:Literal>
                </div>
            </div>

        </div>
    </div>

    <script>
        // Chart Configuration
        document.addEventListener('DOMContentLoaded', function() {
            var ctx = document.getElementById('tasksChart').getContext('2d');
            
            var gradient = ctx.createLinearGradient(0, 0, 0, 400);
            gradient.addColorStop(0, 'rgba(61, 106, 255, 0.5)');
            gradient.addColorStop(1, 'rgba(61, 106, 255, 0.0)');

            var dataPoints = <%= ChartDataJson %>;
            var labels = <%= ChartLabelsJson %>;

            var tasksChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Tasks Completed',
                        data: dataPoints,
                        borderColor: '#3d6aff',
                        backgroundColor: gradient,
                        borderWidth: 2,
                        tension: 0.4, // Smooth curve
                        fill: true,
                        pointBackgroundColor: '#fff',
                        pointBorderColor: '#3d6aff',
                        pointBorderWidth: 2,
                        pointRadius: 4,
                        pointHoverRadius: 6
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: false
                        },
                        tooltip: {
                            backgroundColor: '#fff',
                            titleColor: '#333',
                            bodyColor: '#666',
                            borderColor: '#f0f0f0',
                            borderWidth: 1,
                            padding: 10,
                            displayColors: false
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            grid: {
                                color: '#f5f6fa',
                                drawBorder: false
                            },
                            ticks: {
                                stepSize: 1,
                                color: '#999',
                                font: {
                                    family: "'PlusJakartaSans', sans-serif",
                                    size: 11
                                }
                            }
                        },
                        x: {
                            grid: {
                                display: false
                            },
                            ticks: {
                                color: '#999',
                                font: {
                                    family: "'PlusJakartaSans', sans-serif",
                                    size: 11
                                }
                            }
                        }
                    }
                }
            });
        });
    </script>
</asp:Content>
