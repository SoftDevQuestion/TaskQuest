using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TaskQuest
{
    public partial class SideBar : System.Web.UI.MasterPage
    {
        string connectionString = ConnectionHelper.GetConnectionString();

        protected void Page_Load(object sender, EventArgs e)
        {
            string activePage = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();
            
            // Toggle visibility of project list based on current page
            projectSection.Visible = (activePage == "projects.aspx");
            teamSection.Visible = (activePage == "teams.aspx");
            taskSection.Visible = (activePage == "tasks.aspx");

            if (!IsPostBack)
            {
                UpdateProfileInfo();
            }

            // Always load lists to ensure they are up-to-date after postbacks (e.g. edits)
            if (projectSection.Visible)
            {
                LoadRecentProjects();
            }

            if (teamSection.Visible)
            {
                LoadRecentTeams();
            }
            
            if (taskSection.Visible)
            {
                LoadTaskCounts();
            }
            
            SetActiveMenu();
        }

        public void LoadTaskCounts()
        {
            if (Session["User"] == null) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string username = Session["User"].ToString();
                    
                    // Get UserID for CreatorID check
                    int userId = 0;
                    SqlCommand cmdUser = new SqlCommand("SELECT UserID FROM Users WHERE Username = @Username", conn);
                    cmdUser.Parameters.AddWithValue("@Username", username);
                    object result = cmdUser.ExecuteScalar();
                    if (result != null) userId = Convert.ToInt32(result);

                    // Count tasks from ALL projects the user has access to (Created by user OR User is member of assigned team)
                    string query = @"
                        SELECT 
                            SUM(CASE WHEN StatusId = 3 THEN 1 ELSE 0 END) AS DoneCount,
                            SUM(CASE WHEN StatusId != 3 AND (DueDate >= CAST(GETDATE() AS DATE) OR DueDate IS NULL) THEN 1 ELSE 0 END) AS TodoCount,
                            SUM(CASE WHEN StatusId != 3 AND DueDate < CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END) AS OverdueCount
                        FROM Tasks t
                        WHERE t.ProjectID IN (
                            SELECT DISTINCT p.ProjectID
                            FROM Projects p
                            LEFT JOIN ProjectTeams pt ON p.ProjectID = pt.ProjectID
                            LEFT JOIN Team t ON pt.TeamID = t.TeamId
                            LEFT JOIN TeamMembers tm ON t.TeamId = tm.TeamId
                            WHERE p.CreatorID = @UserID OR tm.Username = @Username
                        )";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblDoneCount.InnerText = reader["DoneCount"] != DBNull.Value ? reader["DoneCount"].ToString() : "0";
                            lblTodoCount.InnerText = reader["TodoCount"] != DBNull.Value ? reader["TodoCount"].ToString() : "0";
                            lblOverdueCount.InnerText = reader["OverdueCount"] != DBNull.Value ? reader["OverdueCount"].ToString() : "0";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error for debugging
                    System.Diagnostics.Debug.WriteLine("Error in LoadTaskCounts: " + ex.Message);
                }
            }
        }

        public void LoadRecentProjects()
        {
            if (Session["User"] == null) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Fetch top 3 recent projects
                    string query = @"
                        SELECT DISTINCT TOP 3 p.ProjectName, p.ProjectLogo, p.CreatedAt
                        FROM Projects p
                        LEFT JOIN ProjectTeams pt ON p.ProjectId = pt.ProjectID
                        LEFT JOIN Team t1 ON pt.TeamID = t1.TeamId
                        LEFT JOIN Team t2 ON p.TeamAccessId = t2.TeamId
                        WHERE t1.CreatorUsername = @Username OR t2.CreatorUsername = @Username
                        ORDER BY p.CreatedAt DESC";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", Session["User"].ToString());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptSideBarProjects.DataSource = dt;
                    rptSideBarProjects.DataBind();
                }
                catch (Exception)
                {
                    // Fail silently
                }
            }
        }

        public void LoadRecentTeams()
        {
            if (Session["User"] == null) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Fetch top 3 recent teams. 
                    // Sorted by UpdatedAt descending to show recently created or edited teams.
                    string query = @"
                        SELECT DISTINCT TOP 3 t.TeamId, t.TeamName, t.LogoPath, t.UpdatedAt
                        FROM Team t
                        LEFT JOIN TeamMembers tm ON t.TeamId = tm.TeamId
                        WHERE t.CreatorUsername = @Username OR tm.Username = @Username
                        ORDER BY t.UpdatedAt DESC";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", Session["User"].ToString());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptSideBarTeams.DataSource = dt;
                    rptSideBarTeams.DataBind();
                }
                catch (Exception)
                {
                    // Fail silently
                }
            }
        }

        public void UpdateProjectList()
        {
            LoadRecentProjects();
            upSideBarProjects.Update();
        }

        private void SetActiveMenu()
        {
            string activePage = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            // Reset all classes
            liDashboard.Attributes["class"] = "";
            liTasks.Attributes["class"] = "";
            liTeams.Attributes["class"] = "";
            liProjects.Attributes["class"] = "";

            switch (activePage)
            {
                case "dashboard.aspx":
                    liDashboard.Attributes["class"] = "active";
                    break;
                case "tasks.aspx":
                    liTasks.Attributes["class"] = "active";
                    break;
                case "teams.aspx":
                    liTeams.Attributes["class"] = "active";
                    break;
                case "projects.aspx":
                    liProjects.Attributes["class"] = "active";
                    break;
            }
        }

        public void UpdateProfileInfo()
        {
            if (Session["User"] != null)
            {
                lblProfileName.Text = Session["User"].ToString();
                
                if (Session["UserAvatar"] != null)
                {
                    imgProfileAvatar.ImageUrl = Session["UserAvatar"].ToString();
                }
                else
                {
                    // Fallback to check DB or use default
                    // For now, let's assume if session is empty, we might need to load it (or just use default)
                    // In a real scenario, you might want to fetch from DB if Session["UserAvatar"] is null but User is logged in
                    // But for performance, relying on Session set during Login/ProfileUpdate is better.
                    imgProfileAvatar.ImageUrl = "assets/images/default-avatar.png";
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();
            // اینجا می‌تونی سرچ رو هندل کنی، مثلاً ریدایرکت یا فیلتر دیتا
            Response.Redirect("SearchResults.aspx?q=" + Server.UrlEncode(query), false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}