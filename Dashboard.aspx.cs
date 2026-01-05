using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Globalization;

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
                        SELECT DISTINCT TOP 3 
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

            // Calculate start of the current week (Monday)
            DateTime today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime startOfWeek = today.AddDays(-1 * diff).Date;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Get completed tasks count for current week (starting Monday)
                    string query = @"
                        SELECT 
                            FORMAT(t.CreatedAt, 'yyyy-MM-dd') as DateStr, -- Using CreatedAt as proxy
                            COUNT(DISTINCT t.TaskID) as TaskCount
                        FROM Tasks t
                        JOIN Projects p ON t.ProjectID = p.ProjectID
                        LEFT JOIN ProjectTeams pt ON p.ProjectID = pt.ProjectID
                        LEFT JOIN Team tm_team ON pt.TeamID = tm_team.TeamId
                        LEFT JOIN TeamMembers tm ON tm_team.TeamId = tm.TeamId
                        WHERE t.StatusId = 3 -- Done
                        AND t.CreatedAt >= @StartDate
                        AND (p.CreatorUserId = @UserId OR tm.Username = @Username)
                        GROUP BY FORMAT(t.CreatedAt, 'yyyy-MM-dd')
                        ORDER BY DateStr";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@StartDate", startOfWeek);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Prepare data for Chart.js
                    List<string> labels = new List<string>();
                    List<int> data = new List<int>();
                    
                    // Fill gaps with 0 for the whole week (Mon-Sun)
                    for (int i = 0; i < 7; i++)
                    {
                        DateTime date = startOfWeek.AddDays(i);
                        string dateStr = date.ToString("yyyy-MM-dd");
                        string displayDate = date.ToString("ddd", CultureInfo.InvariantCulture); // Mon, Tue...

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
            lblCalendarMonth.Text = now.ToString("MMMM yyyy", CultureInfo.InvariantCulture);

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
                html += $"<div class='calendar-day {activeClass}'>{day.ToString("00", CultureInfo.InvariantCulture)}</div>";
            }

            litCalendarDays.Text = html;
        }

        protected string GetDateString(object dateObj)
        {
            if (dateObj != null && dateObj != DBNull.Value)
            {
                return Convert.ToDateTime(dateObj).ToString("MMMM d, yyyy", CultureInfo.InvariantCulture);
            }
            return "";
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
