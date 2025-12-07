<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Teams.aspx.cs" Inherits="TaskQuest.Teams" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>TaskQuest - تیم‌ها</title>
    <link rel="stylesheet" href="assets/css/teams.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="teams-container">
            <nav class="sidebar">
                <div class="logo">
                    <h2>TaskQuest</h2>
                </div>
                <ul class="nav-menu">
                    <li><a href="Dashboard.aspx" class="nav-link">داشبورد</a></li>
                    <li><a href="Tasks.aspx" class="nav-link">وظایف</a></li>
                    <li><a href="Projects.aspx" class="nav-link">پروژه‌ها</a></li>
                    <li><a href="Teams.aspx" class="nav-link active">تیم‌ها</a></li>
                </ul>
                <div class="user-info">
                    <img id="userAvatar" src="assets/images/default-avatar.png" alt="User Avatar" class="user-avatar" />
                    <span id="userName" class="user-name" runat="server">کاربر</span>
                    <a href="Logout.aspx" class="logout-link">خروج</a>
                </div>
            </nav>

            <main class="main-content">
                <header class="content-header">
                    <h1>مدیریت تیم‌ها</h1>
                    <button type="button" class="btn btn-primary" id="createTeamBtn">ایجاد تیم جدید</button>
                </header>

                <div class="teams-content">
                    <div class="teams-list" id="teamsList">
                        <div class="no-teams-message">
                            <p>هنوز تیمی ایجاد نشده است.</p>
                            <button type="button" class="btn btn-outline-primary" onclick="showCreateTeamModal()">اولین تیم را ایجاد کنید</button>
                        </div>
                    </div>
                </div>
            </main>
        </div>

        <!-- Create Team Modal -->
        <div id="createTeamModal" class="modal" style="display: none;">
            <div class="modal-content">
                <div class="modal-header">
                    <h3>ایجاد تیم جدید</h3>
                    <span class="close" onclick="hideCreateTeamModal()">&times;</span>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="teamName">نام تیم:</label>
                        <input type="text" id="teamName" class="form-control" required />
                    </div>
                    <div class="form-group">
                        <label for="teamDescription">توضیحات:</label>
                        <textarea id="teamDescription" class="form-control" rows="3"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="teamMembers">اعضای تیم:</label>
                        <textarea id="teamMembers" class="form-control" rows="2" placeholder="ایمیل اعضا را با کاما جدا کنید"></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="hideCreateTeamModal()">انصراف</button>
                    <button type="button" class="btn btn-primary" onclick="createTeam()">ایجاد تیم</button>
                </div>
            </div>
        </div>
    </form>

    <script src="assets/js/teams.js"></script>
</body>
</html>