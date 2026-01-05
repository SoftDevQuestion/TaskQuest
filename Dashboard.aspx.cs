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
                    // Get all projects user has access to
                    string query = @"
                        SELECT DISTINCT 
                            p.ProjectID, 
                            p.ProjectName, 
                            p.Description, 
                            p.UpdatedAt, 
                            p.CreatedAt,
                            ISNULL(p.DoneStatusCount, 0) as Done,
                            ISNULL(p.InProgressStatusCount, 0) as InProgress,
                            ISNULL(p.ToDoStatusCount, 0) as ToDo,
                            (SELECT MIN(DueDate) FROM Tasks t WHERE t.ProjectID = p.ProjectID AND t.StatusId != 3 AND t.DueDate >= CAST(GETDATE() AS DATE)) as NearestDueDate
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
                    dt.Columns.Add("DaysLeftText", typeof(string));
                    dt.Columns.Add("DaysLeftColor", typeof(string));
                    dt.Columns.Add("DaysLeftTextColor", typeof(string));

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

                        // Days Left Logic (Nearest Task)
                        object nearestDateObj = row["NearestDueDate"];
                        if (nearestDateObj != DBNull.Value)
                        {
                            DateTime nearestDate = Convert.ToDateTime(nearestDateObj);
                            TimeSpan diff = nearestDate - DateTime.Now;
                            int daysLeft = diff.Days + 1; // +1 to include today

                            if (daysLeft <= 0)
                            {
                                row["DaysLeftText"] = "Due today";
                                row["DaysLeftColor"] = "#ffebf0"; // Light Pink
                                row["DaysLeftTextColor"] = "#ff4d88"; // Pink
                            }
                            else
                            {
                                row["DaysLeftText"] = $"{daysLeft} days left";
                                if (daysLeft <= 3)
                                {
                                    row["DaysLeftColor"] = "#ffebf0"; // Light Pink
                                    row["DaysLeftTextColor"] = "#ff4d88"; // Pink
                                }
                                else
                                {
                                    row["DaysLeftColor"] = "#e6fcf5"; // Light Green
                                    row["DaysLeftTextColor"] = "#00d084"; // Green
                                }
                            }
                        }
                        else
                        {
                            // No upcoming tasks
                            row["DaysLeftText"] = "No upcoming tasks";
                            row["DaysLeftColor"] = "#f5f6fa"; // Grey
                            row["DaysLeftTextColor"] = "#999999"; // Dark Grey
                        }

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

            // Calculate 7-day range: Today - 3 days to Today + 3 days
            DateTime today = DateTime.Today;
            DateTime startDate = today.AddDays(-3);
            DateTime endDate = today.AddDays(3);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Fetch tasks relevant to the date range (either DueDate in range OR Completed in range)
                    string query = @"
                        SELECT 
                            t.TaskID,
                            t.StatusId,
                            t.DueDate,
                            t.UpdatedAt
                        FROM Tasks t
                        JOIN Projects p ON t.ProjectID = p.ProjectID
                        LEFT JOIN ProjectTeams pt ON p.ProjectID = pt.ProjectID
                        LEFT JOIN Team tm_team ON pt.TeamID = tm_team.TeamId
                        LEFT JOIN TeamMembers tm ON tm_team.TeamId = tm.TeamId
                        WHERE (p.CreatorUserId = @UserId OR tm.Username = @Username)
                        AND (
                            (t.DueDate >= @StartDate AND t.DueDate <= @EndDate)
                            OR 
                            (t.StatusId = 3 AND t.UpdatedAt >= @StartDate AND t.UpdatedAt <= @EndDate)
                        )";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.AddDays(1).AddSeconds(-1)); // End of the day

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Prepare data for Chart.js
                    List<string> labels = new List<string>();
                    List<double> data = new List<double>();
                    
                    // Fill 7 days centered on Today
                    for (int i = -3; i <= 3; i++)
                    {
                        DateTime date = today.AddDays(i);
                        string displayDate = date.ToString("dddd MM/dd", CultureInfo.InvariantCulture); // e.g., Monday 01/05

                        // Logic:
                        // Total = Tasks Due this day OR Tasks Completed this day
                        // Done = Tasks Completed this day

                        int totalCount = 0;
                        int doneCount = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            bool isDue = false;
                            bool isCompletedToday = false;

                            // Check Due Date
                            if (row["DueDate"] != DBNull.Value)
                            {
                                DateTime due = Convert.ToDateTime(row["DueDate"]).Date;
                                if (due == date) isDue = true;
                            }

                            // Check Completion (Status 3 + UpdatedAt)
                            int status = Convert.ToInt32(row["StatusId"]);
                            if (status == 3 && row["UpdatedAt"] != DBNull.Value)
                            {
                                DateTime updated = Convert.ToDateTime(row["UpdatedAt"]).Date;
                                if (updated == date) isCompletedToday = true;
                            }

                            if (isCompletedToday) doneCount++;
                            if (isDue || isCompletedToday) totalCount++;
                        }

                        double ratio = 0;
                        if (totalCount > 0)
                        {
                            ratio = (double)doneCount / totalCount * 100;
                        }

                        labels.Add(displayDate);
                        data.Add(ratio);
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    ChartLabelsJson = serializer.Serialize(labels);
                    ChartDataJson = serializer.Serialize(data);
                }
                catch 
                {
                    // Fallback
                    ChartLabelsJson = "['Day 1', 'Day 2', 'Day 3', 'Today', 'Day 5', 'Day 6', 'Day 7']";
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
