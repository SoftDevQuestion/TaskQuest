<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tasks.aspx.cs" Inherits="TaskQuest.Tasks" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>TaskQuest - وظایف</title>
    <link rel="stylesheet" href="assets/css/tasks.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="tasks-container">
            <nav class="sidebar">
                <div class="logo">
                    <h2><asp:Label ID="lblLogo" runat="server" Text="TaskQuest"></asp:Label></h2>
                </div>
                <ul class="nav-menu">
                    <li><a href="Dashboard.aspx" class="nav-link"><asp:Label ID="lblNavDashboard" runat="server" Text="داشبورد"></asp:Label></a></li>
                    <li><a href="Tasks.aspx" class="nav-link active"><asp:Label ID="lblNavTasks" runat="server" Text="وظایف"></asp:Label></a></li>
                    <li><a href="Projects.aspx" class="nav-link"><asp:Label ID="lblNavProjects" runat="server" Text="پروژه‌ها"></asp:Label></a></li>
                    <li><a href="Teams.aspx" class="nav-link"><asp:Label ID="lblNavTeams" runat="server" Text="تیم‌ها"></asp:Label></a></li>
                </ul>
                <div class="user-info">
                    <img id="userAvatar" src="assets/images/default-avatar.png" alt="User Avatar" class="user-avatar" />
                    <span id="userName" class="user-name" runat="server"><asp:Label ID="lblUsername" runat="server" Text="کاربر"></asp:Label></span>
                    <a href="Logout.aspx" class="logout-link"><asp:Label ID="lblLogout" runat="server" Text="خروج"></asp:Label></a>
                </div>
            </nav>

            <main class="main-content">
                <header class="content-header">
                    <h1><asp:Label ID="lblPageTitle" runat="server" Text="مدیریت وظایف"></asp:Label></h1>
                    <button type="button" class="btn btn-primary" id="addTaskBtn"><asp:Label ID="lblAddTask" runat="server" Text="افزودن وظیفه جدید"></asp:Label></button>
                </header>

                <div class="tasks-content">
                    <div class="tasks-filters">
                        <div class="filter-group">
                            <label for="statusFilter">وضعیت:</label>
                            <select id="statusFilter" class="form-control">
                                <option value="">همه</option>
                                <option value="pending">در انتظار</option>
                                <option value="in-progress">در حال انجام</option>
                                <option value="completed">تکمیل شده</option>
                            </select>
                        </div>
                        <div class="filter-group">
                            <label for="priorityFilter">اولویت:</label>
                            <select id="priorityFilter" class="form-control">
                                <option value="">همه</option>
                                <option value="high">بالا</option>
                                <option value="medium">متوسط</option>
                                <option value="low">پایین</option>
                            </select>
                        </div>
                    </div>

                    <div class="tasks-list" id="tasksList">
                        <div class="no-tasks-message">
                            <p>هنوز وظیفه‌ای تعریف نشده است.</p>
                            <button type="button" class="btn btn-outline-primary" onclick="showAddTaskModal()">اولین وظیفه را ایجاد کنید</button>
                        </div>
                    </div>
                </div>
            </main>
        </div>

        <!-- Add Task Modal -->
        <div id="addTaskModal" class="modal" style="display: none;">
            <div class="modal-content">
                <div class="modal-header">
                    <h3>افزودن وظیفه جدید</h3>
                    <span class="close" onclick="hideAddTaskModal()">&times;</span>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="taskTitle">عنوان وظیفه:</label>
                        <input type="text" id="taskTitle" class="form-control" required />
                    </div>
                    <div class="form-group">
                        <label for="taskDescription">توضیحات:</label>
                        <textarea id="taskDescription" class="form-control" rows="3"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="taskPriority">اولویت:</label>
                        <select id="taskPriority" class="form-control">
                            <option value="low">پایین</option>
                            <option value="medium" selected>متوسط</option>
                            <option value="high">بالا</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="taskDueDate">موعد تحویل:</label>
                        <input type="date" id="taskDueDate" class="form-control" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="hideAddTaskModal()">انصراف</button>
                    <button type="button" class="btn btn-primary" onclick="addTask()">افزودن وظیفه</button>
                </div>
            </div>
        </div>
    </form>

    <script src="assets/js/tasks.js"></script>
</body>
</html>