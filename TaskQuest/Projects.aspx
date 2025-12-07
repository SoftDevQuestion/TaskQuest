<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="TaskQuest.Projects" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>TaskQuest - پروژه‌ها</title>
    <link rel="stylesheet" href="assets/css/projects.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="projects-container">
            <nav class="sidebar">
                <div class="logo">
                    <h2>TaskQuest</h2>
                </div>
                <ul class="nav-menu">
                    <li><a href="Dashboard.aspx" class="nav-link">داشبورد</a></li>
                    <li><a href="Tasks.aspx" class="nav-link">وظایف</a></li>
                    <li><a href="Projects.aspx" class="nav-link active">پروژه‌ها</a></li>
                    <li><a href="Teams.aspx" class="nav-link">تیم‌ها</a></li>
                </ul>
                <div class="user-info">
                    <img id="userAvatar" src="assets/images/default-avatar.png" alt="User Avatar" class="user-avatar" />
                    <span id="userName" class="user-name" runat="server">کاربر</span>
                    <a href="Logout.aspx" class="logout-link">خروج</a>
                </div>
            </nav>

            <main class="main-content">
                <header class="content-header">
                    <h1>مدیریت پروژه‌ها</h1>
                    <button type="button" class="btn btn-primary" id="addProjectBtn">ایجاد پروژه جدید</button>
                </header>

                <div class="projects-content">
                    <div class="projects-grid" id="projectsGrid">
                        <div class="no-projects-message">
                            <p>هنوز پروژه‌ای ایجاد نشده است.</p>
                            <button type="button" class="btn btn-outline-primary" onclick="showAddProjectModal()">اولین پروژه را ایجاد کنید</button>
                        </div>
                    </div>
                </div>
            </main>
        </div>

        <!-- Add Project Modal -->
        <div id="addProjectModal" class="modal" style="display: none;">
            <div class="modal-content">
                <div class="modal-header">
                    <h3>ایجاد پروژه جدید</h3>
                    <span class="close" onclick="hideAddProjectModal()">&times;</span>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="projectName">نام پروژه:</label>
                        <input type="text" id="projectName" class="form-control" required />
                    </div>
                    <div class="form-group">
                        <label for="projectDescription">توضیحات:</label>
                        <textarea id="projectDescription" class="form-control" rows="3"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="projectStartDate">تاریخ شروع:</label>
                        <input type="date" id="projectStartDate" class="form-control" />
                    </div>
                    <div class="form-group">
                        <label for="projectEndDate">تاریخ پایان:</label>
                        <input type="date" id="projectEndDate" class="form-control" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="hideAddProjectModal()">انصراف</button>
                    <button type="button" class="btn btn-primary" onclick="addProject()">ایجاد پروژه</button>
                </div>
            </div>
        </div>
    </form>

    <script src="assets/js/projects.js"></script>
</body>
</html>