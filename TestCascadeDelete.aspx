<%@ Page Language="C#" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Web.Configuration" %>
<!DOCTYPE html>
<html>
<head>
    <title>Cascade Delete Test</title>
    <style>
        body { font-family: sans-serif; padding: 20px; }
        .success { color: green; }
        .fail { color: red; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Cascade Delete Unit Test</h1>
        <asp:Button ID="btnRunTest" runat="server" Text="Run Test" OnClick="btnRunTest_Click" />
        <br /><br />
        <asp:Label ID="lblResult" runat="server" Text="Ready..."></asp:Label>
    </form>
    <script runat="server">
        string connStr = TaskQuest.ConnectionHelper.GetConnectionString();

        protected void btnRunTest_Click(object sender, EventArgs e)
        {
            lblResult.Text = "Starting Test...<br/>";
            string testUser = "TestUser_" + DateTime.Now.Ticks;
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction trans = null; // We use SP's transaction for delete, but here we setup data.

                try
                {
                    // 1. Create User
                    string createUser = "INSERT INTO Users (Username, PasswordHash, Email) VALUES (@User, 'hash', @User + '@test.com'); SELECT SCOPE_IDENTITY();"; // Assuming simple users table or similar
                    // Note: If Users table has other required fields, this might fail.
                    // Let's assume standard fields.
                    
                    // Actually, 'Users' table schema from InspectColumns: UserID, FullName, Username, Email...
                    createUser = "INSERT INTO Users (Username, PasswordHash, Email, FullName) VALUES (@User, 'hash', @User + '@test.com', 'Test User');";
                    SqlCommand cmdUser = new SqlCommand(createUser, conn);
                    cmdUser.Parameters.AddWithValue("@User", testUser);
                    cmdUser.ExecuteNonQuery();
                    Log("User created: " + testUser);

                    // 2. Create Team
                    string createTeam = "INSERT INTO Team (TeamName, CreatorUsername, UpdatedAt) VALUES ('Test Team ' + @User, @User, GETDATE()); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdTeam = new SqlCommand(createTeam, conn);
                    cmdTeam.Parameters.AddWithValue("@User", testUser);
                    int teamId = Convert.ToInt32(cmdTeam.ExecuteScalar());
                    Log("Team created: " + teamId);

                    // 3. Create Project (Linked to Team via TeamAccessId)
                    // Projects(ProjectId, CreatorUserId, TeamAccessId...)
                    // We need UserID for CreatorUserId? Or can be null?
                    // Let's get UserID.
                    string getUserId = "SELECT UserID FROM Users WHERE Username = @User";
                    SqlCommand cmdGetId = new SqlCommand(getUserId, conn);
                    cmdGetId.Parameters.AddWithValue("@User", testUser);
                    int userId = (int)cmdGetId.ExecuteScalar();

                    string createProject = "INSERT INTO Projects (ProjectName, TeamAccessId, CreatorUserId, CreatedAt) VALUES ('Test Project', @TeamId, @UserId, GETDATE()); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdProj = new SqlCommand(createProject, conn);
                    cmdProj.Parameters.AddWithValue("@TeamId", teamId);
                    cmdProj.Parameters.AddWithValue("@UserId", userId);
                    int projId = Convert.ToInt32(cmdProj.ExecuteScalar());
                    Log("Project created: " + projId);

                    // 4. Create Task
                    string createTask = "INSERT INTO Tasks (ProjectId, Title, StatusId) VALUES (@ProjId, 'Test Task', 1);";
                    SqlCommand cmdTask = new SqlCommand(createTask, conn);
                    cmdTask.Parameters.AddWithValue("@ProjId", projId);
                    cmdTask.ExecuteNonQuery();
                    Log("Task created.");

                    // 5. Run Cascade Delete SP
                    Log("Executing sp_DeleteUserCascade...");
                    string runSp = "sp_DeleteUserCascade";
                    SqlCommand cmdSp = new SqlCommand(runSp, conn);
                    cmdSp.CommandType = CommandType.StoredProcedure;
                    cmdSp.Parameters.AddWithValue("@Username", testUser);
                    cmdSp.ExecuteNonQuery();
                    Log("SP Executed.");

                    // 6. Verify Deletion
                    bool userDeleted = (int)(new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = '" + testUser + "'", conn).ExecuteScalar()) == 0;
                    bool teamDeleted = (int)(new SqlCommand("SELECT COUNT(*) FROM Team WHERE TeamId = " + teamId, conn).ExecuteScalar()) == 0;
                    bool projDeleted = (int)(new SqlCommand("SELECT COUNT(*) FROM Projects WHERE ProjectId = " + projId, conn).ExecuteScalar()) == 0;
                    
                    if (userDeleted && teamDeleted && projDeleted)
                    {
                        lblResult.Text += "<b class='success'>Test PASSED!</b> All related data deleted.";
                    }
                    else
                    {
                        lblResult.Text += "<b class='fail'>Test FAILED!</b><br/>";
                        if (!userDeleted) lblResult.Text += "- User not deleted<br/>";
                        if (!teamDeleted) lblResult.Text += "- Team not deleted<br/>";
                        if (!projDeleted) lblResult.Text += "- Project not deleted<br/>";
                    }

                }
                catch (Exception ex)
                {
                    lblResult.Text += "<b class='fail'>Error:</b> " + ex.Message;
                }
            }
        }

        private void Log(string msg)
        {
            lblResult.Text += msg + "<br/>";
        }
    </script>
</body>
</html>
