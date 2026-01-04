using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

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
                    // Fetch tasks for the specific project
                    string query = "SELECT * FROM Tasks WHERE ProjectID = @ProjectID ORDER BY DueDate ASC";
                    
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
        protected string GetStatusClass(object statusIdObj)
        {
            if (statusIdObj == DBNull.Value) return "";
            
            int statusId = Convert.ToInt32(statusIdObj);
            
            switch (statusId)
            {
                case 1: return ""; // To Do (default)
                case 2: return "status-in-progress";
                case 3: return "status-done";
                default: return "";
            }
        }

        // Helper to get status text
        protected string GetStatusText(object statusIdObj)
        {
            if (statusIdObj == DBNull.Value) return "Undefined";
            
            int statusId = Convert.ToInt32(statusIdObj);
            
            switch (statusId)
            {
                case 1: return "To Do";
                case 2: return "In Progress";
                case 3: return "Done";
                default: return "Undefined";
            }
        }
        
        // Helper to get status class for Overdue logic (optional/advanced)
        // Currently relying on basic status mapping. 
        // Overdue logic would require checking DueDate vs DateTime.Now and Status != Done.
    }
}
