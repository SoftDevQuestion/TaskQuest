using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Globalization;

namespace TaskQuest
{
    public partial class Tasks : BasePage
    {
        string connectionString = ConnectionHelper.GetConnectionString();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                EnsureProjectTeamsTable();
                LoadProjects();
                LoadAllUsers();
            }
        }

        private void LoadAllUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT UserID, Username, FullName, AvatarPath FROM Users";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptAssignees.DataSource = dt;
                    rptAssignees.DataBind();

                    rptAssigneesEdit.DataSource = dt;
                    rptAssigneesEdit.DataBind();
                }
                catch (Exception)
                {
                    // Handle error
                }
            }
        }

        private void EnsureProjectTeamsTable()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProjectTeams' AND xtype='U')
                        BEGIN
                            CREATE TABLE ProjectTeams (
                                ProjectTeamID INT IDENTITY(1,1) PRIMARY KEY,
                                ProjectID INT NOT NULL,
                                TeamID INT NOT NULL,
                                FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID) ON DELETE CASCADE,
                                FOREIGN KEY (TeamID) REFERENCES Team(TeamId) ON DELETE CASCADE
                            );
                        END";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                }
                catch { }
            }
        }

        private void LoadProjects()
        {
            if (Session["User"] == null) return;
            string currentUser = Session["User"].ToString();
            int currentUserId = GetUserId(currentUser);
            
            string projectIdFilter = Request.QueryString["ProjectId"];

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Fetch projects where user is creator OR member of assigned team
                    string query = @"
                        SELECT DISTINCT p.*,
                            (SELECT COUNT(*) FROM Tasks t WHERE t.ProjectID = p.ProjectID AND t.StatusId = 3) AS DoneStatusCount,
                            (SELECT COUNT(*) FROM Tasks t WHERE t.ProjectID = p.ProjectID AND t.StatusId = 2) AS InProgressStatusCount,
                            (SELECT COUNT(*) FROM Tasks t WHERE t.ProjectID = p.ProjectID AND t.StatusId = 1) AS ToDoStatusCount
                        FROM Projects p
                        LEFT JOIN ProjectTeams pt ON p.ProjectID = pt.ProjectID
                        LEFT JOIN TeamMembers tm ON pt.TeamID = tm.TeamId
                        WHERE (tm.Username = @CurrentUser 
                           OR p.CreatorUserId = @CurrentUserId)";

                    if (!string.IsNullOrEmpty(projectIdFilter) && int.TryParse(projectIdFilter, out int pId))
                    {
                        query += " AND p.ProjectID = @FilterProjectID";
                    }

                    query += " ORDER BY p.CreatedAt DESC";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CurrentUser", currentUser);
                    cmd.Parameters.AddWithValue("@CurrentUserId", currentUserId);
                    
                    if (!string.IsNullOrEmpty(projectIdFilter) && int.TryParse(projectIdFilter, out int pIdVal))
                    {
                        cmd.Parameters.AddWithValue("@FilterProjectID", pIdVal);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rptProjects.DataSource = dt;
                        rptProjects.DataBind();
                        lblNoProjects.Visible = false;
                    }
                    else
                    {
                        rptProjects.DataSource = null;
                        rptProjects.DataBind();
                        lblNoProjects.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    // Log error
                    lblNoProjects.Text = "Error loading projects: " + ex.Message;
                    lblNoProjects.Visible = true;
                }
            }
        }

        protected void rptProjects_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Find the nested repeater
                Repeater rptTasks = (Repeater)e.Item.FindControl("rptTasks");
                
                // Get the ProjectID from the current data item
                DataRowView drv = (DataRowView)e.Item.DataItem;
                int projectId = Convert.ToInt32(drv["ProjectID"]);

                // Fetch tasks for this project
                LoadTasksForProject(rptTasks, projectId);
            }
        }

        private void LoadTasksForProject(Repeater rptTasks, int projectId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Fetch tasks with Assignee details
                    string query = @"
                        SELECT t.*, u.Username, u.FullName, u.AvatarPath 
                        FROM Tasks t
                        LEFT JOIN Users u ON t.UserID = u.UserID
                        WHERE t.ProjectID = @ProjectID 
                        ORDER BY t.DueDate ASC";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptTasks.DataSource = dt;
                    rptTasks.DataBind();
                }
                catch (Exception)
                {
                    // Handle or log error
                }
            }
        }

        [System.Web.Services.WebMethod]
        public static string CreateNewTask(string projectId, string title, string dueDate, string assigneeUsername)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(projectId))
                return "Error: Missing required fields.";

            string connectionString = ConnectionHelper.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    
                    int? assigneeId = null;
                    if (!string.IsNullOrEmpty(assigneeUsername))
                    {
                        SqlCommand cmdUser = new SqlCommand("SELECT UserID FROM Users WHERE Username = @Username", conn);
                        cmdUser.Parameters.AddWithValue("@Username", assigneeUsername);
                        object result = cmdUser.ExecuteScalar();
                        if (result != null) assigneeId = Convert.ToInt32(result);
                    }

                    string query = @"
                        INSERT INTO Tasks (ProjectID, Title, StatusId, DueDate, UserID, CreatedAt, UpdatedAt) 
                        VALUES (@ProjectID, @Title, 1, @DueDate, @UserID, GETDATE(), GETDATE());
                        
                        UPDATE Projects SET UpdatedAt = GETDATE() WHERE ProjectID = @ProjectID;
                    ";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    cmd.Parameters.AddWithValue("@Title", title);
                    
                    if (string.IsNullOrEmpty(dueDate))
                        cmd.Parameters.AddWithValue("@DueDate", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@DueDate", DateTime.Parse(dueDate, CultureInfo.InvariantCulture));

                    if (assigneeId.HasValue)
                        cmd.Parameters.AddWithValue("@UserID", assigneeId.Value);
                    else
                        cmd.Parameters.AddWithValue("@UserID", DBNull.Value);

                    cmd.ExecuteNonQuery();
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "Error: " + ex.Message;
                }
            }
        }

        [System.Web.Services.WebMethod]
        public static string ToggleTaskStatus(int taskId, bool isDone)
        {
            string connectionString = ConnectionHelper.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // 3 = Done, 1 = To Do
                    int newStatus = isDone ? 3 : 1; 
                    
                    string query = @"
                        UPDATE Tasks SET StatusId = @StatusId WHERE TaskID = @TaskID;
                        UPDATE Projects SET UpdatedAt = GETDATE() WHERE ProjectID = (SELECT ProjectID FROM Tasks WHERE TaskID = @TaskID);
                    ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StatusId", newStatus);
                    cmd.Parameters.AddWithValue("@TaskID", taskId);
                    
                    cmd.ExecuteNonQuery();
                    return "Success";
                }
                catch (Exception ex)
                {
                    // Try TaskId if TaskID fails (fallback logic not really possible here without retry)
                    return "Error: " + ex.Message;
                }
            }
        }

        [System.Web.Services.WebMethod]
        public static object GetTaskDetails(int taskId)
        {
            string connectionString = ConnectionHelper.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT t.Title, t.DueDate, u.Username, u.FullName, u.AvatarPath 
                        FROM Tasks t
                        LEFT JOIN Users u ON t.UserID = u.UserID
                        WHERE t.TaskID = @TaskID";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TaskID", taskId);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return new {
                            Title = reader["Title"].ToString(),
                            DueDate = reader["DueDate"] != DBNull.Value ? Convert.ToDateTime(reader["DueDate"]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                            AssigneeUsername = reader["Username"] != DBNull.Value ? reader["Username"].ToString() : "",
                            AssigneeFullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "Unassigned",
                            AssigneeAvatar = reader["AvatarPath"] != DBNull.Value ? reader["AvatarPath"].ToString() : ""
                        };
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return new { Error = ex.Message };
                }
            }
        }

        [System.Web.Services.WebMethod]
        public static string UpdateTask(int taskId, string title, string dueDate, string assigneeUsername)
        {
            if (string.IsNullOrEmpty(title))
                return "Error: Title is required.";

            string connectionString = ConnectionHelper.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    int? assigneeId = null;
                    if (!string.IsNullOrEmpty(assigneeUsername))
                    {
                        SqlCommand cmdUser = new SqlCommand("SELECT UserID FROM Users WHERE Username = @Username", conn);
                        cmdUser.Parameters.AddWithValue("@Username", assigneeUsername);
                        object result = cmdUser.ExecuteScalar();
                        if (result != null) assigneeId = Convert.ToInt32(result);
                    }

                    string query = @"
                                     UPDATE Tasks 
                                     SET Title = @Title, 
                                         DueDate = @DueDate, 
                                         UserID = @UserID,
                                         UpdatedAt = GETDATE()
                                     WHERE TaskID = @TaskID;

                                     UPDATE Projects SET UpdatedAt = GETDATE() WHERE ProjectID = (SELECT ProjectID FROM Tasks WHERE TaskID = @TaskID);
                                    ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TaskID", taskId);
                    cmd.Parameters.AddWithValue("@Title", title);

                    if (string.IsNullOrEmpty(dueDate))
                        cmd.Parameters.AddWithValue("@DueDate", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@DueDate", DateTime.Parse(dueDate, CultureInfo.InvariantCulture));

                    if (assigneeId.HasValue)
                        cmd.Parameters.AddWithValue("@UserID", assigneeId.Value);
                    else
                        cmd.Parameters.AddWithValue("@UserID", DBNull.Value);

                    cmd.ExecuteNonQuery();
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "Error: " + ex.Message;
                }
            }
        }

        // Helper method to calculate progress percentage
        protected int GetProgressPercentage(object dataItem)
        {
            DataRowView drv = (DataRowView)dataItem;
            
            int done = drv["DoneStatusCount"] != DBNull.Value ? Convert.ToInt32(drv["DoneStatusCount"]) : 0;
            int inProgress = drv["InProgressStatusCount"] != DBNull.Value ? Convert.ToInt32(drv["InProgressStatusCount"]) : 0;
            int todo = drv["ToDoStatusCount"] != DBNull.Value ? Convert.ToInt32(drv["ToDoStatusCount"]) : 0;
            
            int total = done + inProgress + todo;

            if (total == 0) return 0;

            return (int)((double)done / total * 100);
        }

        // Helper to get status class for CSS
        protected string GetStatusClass(object statusIdObj, object dueDateObj)
        {
            // Default to "To Do" if status is missing
            if (statusIdObj == DBNull.Value || statusIdObj == null) return "status-to-do";
            
            int statusId = Convert.ToInt32(statusIdObj);
            
            // 1. Done (Highest Priority for visual state if completed)
            if (statusId == 3) return "status-done";

            // 2. Overdue (If not done, and date is past)
            if (dueDateObj != DBNull.Value && dueDateObj != null)
            {
                DateTime dueDate = Convert.ToDateTime(dueDateObj);
                if (dueDate.Date < DateTime.Now.Date)
                {
                    return "status-overdue";
                }
            }

            // 3. In Progress
            if (statusId == 2) return "status-in-progress";

            // 4. Default To Do
            return "status-to-do";
        }

        // Helper to get status text
        protected string GetStatusText(object statusIdObj, object dueDateObj)
        {
            if (statusIdObj == DBNull.Value || statusIdObj == null) return "To Do";
            
            int statusId = Convert.ToInt32(statusIdObj);
            
            // 1. Done
            if (statusId == 3) return "Done";

            // 2. Overdue
            if (dueDateObj != DBNull.Value && dueDateObj != null)
            {
                DateTime dueDate = Convert.ToDateTime(dueDateObj);
                if (dueDate.Date < DateTime.Now.Date)
                {
                    return "Overdue";
                }
            }
            
            // 3. In Progress
            if (statusId == 2) return "In Progress";
            
            // 4. Default
            return "To Do";
        }
        
        // Helper to get status class for Overdue logic (optional/advanced)
        // Currently relying on basic status mapping. 
        // Overdue logic would require checking DueDate vs DateTime.Now and Status != Done.
    }
}
