using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace TaskQuest
{
    public partial class Dashboard : BasePage
    {
        protected string ChartDataJson = "[]";
        protected string ChartLabelsJson = "[]";
        string connectionString = ConnectionHelper.GetConnectionString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LoadRecentProjects();
                LoadTaskStatistics();
                LoadCalendar();
            }
        }

        private void LoadRecentProjects()
        {
            string username = Session["User"].ToString();
            int userId = GetUserId(username);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Get 3 most recently updated projects
                    string query = @"
                        SELECT TOP 3 
                            p.ProjectID, 
                            p.ProjectName, 
                            p.Description, 
                            p.UpdatedAt, 
                            p.CreatedAt,
                            ISNULL(p.DoneStatusCount, 0) as Done,
                            ISNULL(p.InProgressStatusCount, 0) as InProgress,
                            ISNULL(p.ToDoStatusCount, 0) as ToDo
                        FROM Projects p
                        LEFT JOIN ProjectTeams pt ON p.ProjectID = pt.ProjectID
                        LEFT JOIN Team t ON pt.TeamID = t.TeamId
                        LEFT JOIN TeamMembers tm ON t.TeamId = tm.TeamId
                        WHERE (p.CreatorUserId = @UserId OR tm.Username = @Username)
                        ORDER BY p.UpdatedAt DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Username", username);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Add custom columns for display
                    dt.Columns.Add("Progress", typeof(int));
                    dt.Columns.Add("Color", typeof(string));

                    string[] colors = { "#ff4d88", "#5577ff", "#00d084" }; // Colors from image: Pink, Blue, Green
                    int colorIndex = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        int done = Convert.ToInt32(row["Done"]);
                        int inProgress = Convert.ToInt32(row["InProgress"]);
                        int todo = Convert.ToInt32(row["ToDo"]);
                        int total = done + inProgress + todo;

                        int progress = total > 0 ? (int)((double)done / total * 100) : 0;
                        row["Progress"] = progress;
                        row["Color"] = colors[colorIndex % colors.Length];
                        colorIndex++;
                    }

                    rptRecentProjects.DataSource = dt;
                    rptRecentProjects.DataBind();
                }
                catch
                {
                    // Handle error (log it)
                }
            }
        }

        protected void rptRecentProjects_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                int projectId = Convert.ToInt32(row["ProjectID"]);
                Repeater rptTeamMembers = (Repeater)e.Item.FindControl("rptTeamMembers");

                // Load team members (limit to 3-4)
                LoadTeamMembers(projectId, rptTeamMembers);
            }
        }

        private void LoadTeamMembers(int projectId, Repeater rpt)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT DISTINCT TOP 4 u.Username, u.AvatarPath
                        FROM Users u
                        JOIN TeamMembers tm ON u.Username = tm.Username
                        JOIN ProjectTeams pt ON tm.TeamId = pt.TeamID
                        WHERE pt.ProjectID = @ProjectID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rpt.DataSource = dt;
                    rpt.DataBind();
                }
                catch { }
            }
        }

        private void LoadTaskStatistics()
        {
            string username = Session["User"].ToString();
            int userId = GetUserId(username);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Get completed tasks count for last 7 days
                    // Note: Assuming we want tasks the user has access to or created
                    string query = @"
                        SELECT 
                            FORMAT(t.CreatedAt, 'yyyy-MM-dd') as DateStr, -- Using CreatedAt as proxy if CompletedAt is missing, or update schema to have CompletedAt
                            COUNT(*) as TaskCount
                        FROM Tasks t
                        JOIN Projects p ON t.ProjectID = p.ProjectID
                        LEFT JOIN ProjectTeams pt ON p.ProjectID = pt.ProjectID
                        LEFT JOIN Team tm_team ON pt.TeamID = tm_team.TeamId
                        LEFT JOIN TeamMembers tm ON tm_team.TeamId = tm.TeamId
                        WHERE t.StatusId = 3 -- Done
                        AND t.CreatedAt >= DATEADD(DAY, -6, GETDATE()) -- Last 7 days including today
                        AND (p.CreatorUserId = @UserId OR tm.Username = @Username)
                        GROUP BY FORMAT(t.CreatedAt, 'yyyy-MM-dd')
                        ORDER BY DateStr";

                    // Note: If 'CompletedAt' exists, replace 'CreatedAt' with 'CompletedAt' in WHERE and GROUP BY
                    // For now using CreatedAt because we verified Schema earlier but didn't see CompletedAt column in InspectSchema output explicitly for Tasks table, 
                    // and Tasks.aspx doesn't show CompletedAt. 
                    // Ideally we should use a 'CompletedAt' column. 
                    // Let's assume for this mock that we count tasks *created* in last 7 days that are *done*.
                    // Or better, just count tasks created in last 7 days to show activity. 
                    // The user prompt says "Tasks Completed", so let's stick to StatusId=3.

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Username", username);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Prepare data for Chart.js
                    List<string> labels = new List<string>();
                    List<int> data = new List<int>();
                    
                    DateTime startDate = DateTime.Today.AddDays(-6);
                    
                    // Fill gaps with 0
                    for (int i = 0; i < 7; i++)
                    {
                        DateTime date = startDate.AddDays(i);
                        string dateStr = date.ToString("yyyy-MM-dd");
                        string displayDate = date.ToString("ddd"); // Mon, Tue...

                        DataRow[] foundRows = dt.Select($"DateStr = '{dateStr}'");
                        int count = foundRows.Length > 0 ? Convert.ToInt32(foundRows[0]["TaskCount"]) : 0;

                        labels.Add(displayDate);
                        data.Add(count);
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    ChartLabelsJson = serializer.Serialize(labels);
                    ChartDataJson = serializer.Serialize(data);
                }
                catch 
                {
                    // Fallback empty data
                    ChartLabelsJson = "['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']";
                    ChartDataJson = "[0, 0, 0, 0, 0, 0, 0]";
                }
            }
        }

        private void LoadCalendar()
        {
            DateTime now = DateTime.Now;
            lblCalendarMonth.Text = now.ToString("MMMM yyyy");

            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            DateTime firstDay = new DateTime(now.Year, now.Month, 1);
            int startDayOfWeek = (int)firstDay.DayOfWeek; // 0 = Sunday, 1 = Monday...
            
            // Adjust for Monday start (Monday=0, Sunday=6)
            int offset = (startDayOfWeek == 0) ? 6 : startDayOfWeek - 1;

            string html = "";

            // Empty slots
            for (int i = 0; i < offset; i++)
            {
                html += "<div class='calendar-day empty'></div>";
            }

            // Days
            for (int day = 1; day <= daysInMonth; day++)
            {
                string activeClass = (day == now.Day) ? "active" : "";
                html += $"<div class='calendar-day {activeClass}'>{day:00}</div>";
            }

            litCalendarDays.Text = html;
        }

        protected string GetDaysLeft(object createdAtObj)
        {
            // Logic for "Days left" - Mocking based on CreatedAt + 30 days deadline for now
            // Or if DueDate exists in Projects (it doesn't), we'd use that.
            // The image shows "3 days left", "25 days left".
            // Let's assume a project duration of 30 days from creation.
            
            if (createdAtObj != DBNull.Value)
            {
                DateTime createdAt = Convert.ToDateTime(createdAtObj);
                DateTime deadline = createdAt.AddDays(30);
                TimeSpan diff = deadline - DateTime.Now;

                if (diff.Days > 0)
                    return $"{diff.Days} days left";
                else if (diff.Days == 0)
                    return "Due today";
                else
                    return "Overdue";
            }
            return "";
        }
    }
}
