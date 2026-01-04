using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace TaskQuest
{
    public partial class Projects : System.Web.UI.Page
    {
        string connectionString = ConnectionHelper.GetConnectionString();
        protected int CurrentUserId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            CurrentUserId = GetUserId(Session["User"].ToString());

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
                catch (Exception ex)
                {
                    Log($"Error creating ProjectTeams table: {ex.Message}");
                }
            }
        }

        private void Log(string message)
        {
            try
            {
                string logPath = Server.MapPath("~/debug_log.txt");
                File.AppendAllText(logPath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch { }
        }

        protected void rptProjects_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                string projectId = e.CommandArgument.ToString();
                LoadProjectForEdit(projectId);
            }
            else if (e.CommandName == "Access")
            {
                string projectId = e.CommandArgument.ToString();
                hfAccessProjectId.Value = projectId;
                LoadProjectAccess(projectId);
                ClientScript.RegisterStartupScript(this.GetType(), "OpenAccessModal", "openAccessModal();", true);
            }
        }

        private void LoadProjectAccess(string projectId)
        {
            if (Session["User"] == null) return;
            string username = Session["User"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try 
                {
                    conn.Open();

                    // Get Teams User is member of
                    string teamQuery = @"
                        SELECT t.TeamId, t.TeamName 
                        FROM Team t
                        JOIN TeamMembers tm ON t.TeamId = tm.TeamId
                        WHERE tm.Username = @Username";
                    
                    SqlCommand cmd = new SqlCommand(teamQuery, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dtTeams = new DataTable();
                    da.Fill(dtTeams);

                    cblTeams.DataSource = dtTeams;
                    cblTeams.DataBind();

                    // Mark teams that already have access
                    string accessQuery = "SELECT TeamID FROM ProjectTeams WHERE ProjectID = @ProjectID";
                    SqlCommand accessCmd = new SqlCommand(accessQuery, conn);
                    accessCmd.Parameters.AddWithValue("@ProjectID", projectId);
                    
                    List<int> existingTeamIds = new List<int>();
                    using (SqlDataReader reader = accessCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            existingTeamIds.Add(reader.GetInt32(0));
                        }
                    }

                    foreach (System.Web.UI.WebControls.ListItem item in cblTeams.Items)
                    {
                        if (existingTeamIds.Contains(int.Parse(item.Value)))
                        {
                            item.Selected = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log("Error loading project access: " + ex.Message);
                }
            }
        }

        protected void btnSaveAccess_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfAccessProjectId.Value)) return;
            int projectId = int.Parse(hfAccessProjectId.Value);
            if (Session["User"] == null) return;
            string username = Session["User"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                
                try
                {
                    // Delete existing access for visible teams
                    string deleteQuery = @"
                        DELETE FROM ProjectTeams 
                        WHERE ProjectID = @ProjectID 
                        AND TeamID IN (SELECT TeamId FROM TeamMembers WHERE Username = @Username)";
                    
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction);
                    deleteCmd.Parameters.AddWithValue("@ProjectID", projectId);
                    deleteCmd.Parameters.AddWithValue("@Username", username);
                    deleteCmd.ExecuteNonQuery();

                    // Insert selected teams
                    string insertQuery = "INSERT INTO ProjectTeams (ProjectID, TeamID) VALUES (@ProjectID, @TeamID)";
                    
                    foreach (System.Web.UI.WebControls.ListItem item in cblTeams.Items)
                    {
                        if (item.Selected)
                        {
                            SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction);
                            insertCmd.Parameters.AddWithValue("@ProjectID", projectId);
                            insertCmd.Parameters.AddWithValue("@TeamID", int.Parse(item.Value));
                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    
                    LoadProjects();
                    
                    ClientScript.RegisterStartupScript(this.GetType(), "CloseAccessModal", "closeAccessModal();", true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblAccessError.Text = "Error saving access: " + ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "OpenAccessModal", "openAccessModal();", true);
                }
            }
        }

        private void LoadProjectForEdit(string projectId)
        {
            int pId = int.Parse(projectId);
            DataRow project = GetProjectById(pId);
            
            if (project != null)
            {
                hfEditProjectId.Value = projectId;
                txtProjectName.Text = project["ProjectName"].ToString();
                txtDescription.Text = project["Description"].ToString();
                
                string logo = project["ProjectLogo"].ToString();
                string cover = project["ProjectCover"].ToString();
                
                imgLogoPreview.ImageUrl = string.IsNullOrEmpty(logo) ? "assets/images/projectTestAvatar.jpg" : logo;
                imgCoverPreview.ImageUrl = string.IsNullOrEmpty(cover) ? "assets/images/default_cover.jpg" : cover;
                
                lblModalTitle.Text = "Edit Project";
                btnVisibleCreate.InnerText = "Save Changes";
                
                KeepModalOpen();
            }
        }

        private DataRow GetProjectById(int projectId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Projects WHERE ProjectID = @ProjectID", conn);
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0) return dt.Rows[0];
                }
                catch (Exception ex)
                {
                    Log($"Error in GetProjectById: {ex.Message}");
                }
            }
            return null;
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
             if (!string.IsNullOrEmpty(hfDeleteProjectId.Value))
             {
                 int projectId = int.Parse(hfDeleteProjectId.Value);
                 DeleteProjectFromDb(projectId);
                 LoadProjects();
                 hfDeleteProjectId.Value = string.Empty;
                 
                 // Update Sidebar
                 if (Master is SideBar sideBar)
                 {
                     sideBar.UpdateProjectList();
                 }

                 ClientScript.RegisterStartupScript(this.GetType(), "CloseDeleteModal", "closeDeleteModal();", true);
             }
        }

        private void DeleteProjectFromDb(int projectId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Projects WHERE ProjectID = @ProjectID", conn);
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log($"Error deleting project: {ex.Message}");
                }
            }
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            Log("btnCreateProject_Click started.");
            try
            {
                if (Session["User"] == null)
                {
                    Log("Session['User'] is null. Redirecting to Login.");
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                string username = Session["User"].ToString();
                string projectName = txtProjectName.Text.Trim();
                string description = txtDescription.Text.Trim();
                int userId = GetUserId(username);

                if (string.IsNullOrEmpty(projectName))
                {
                    ShowError("Project Name is required");
                    KeepModalOpen();
                    return;
                }

                if (userId == -1)
                {
                    ShowError("User not found. Please re-login.");
                    KeepModalOpen();
                    return;
                }

                // Check if it's Edit or Create
                if (!string.IsNullOrEmpty(hfEditProjectId.Value))
                {
                    // EDIT MODE
                    int projectId = int.Parse(hfEditProjectId.Value);
                    
                    // Check Uniqueness (exclude current project)
                    if (IsProjectNameTaken(userId, projectName, projectId))
                    {
                        ShowError("Project name already exists for this user.");
                        KeepModalOpen();
                        return;
                    }

                    // Get existing paths
                    DataRow existing = GetProjectById(projectId);
                    string logoPath = existing["ProjectLogo"].ToString();
                    string coverPath = existing["ProjectCover"].ToString();

                    if (fuProjectLogo.HasFile) logoPath = UploadFile(fuProjectLogo, "logo");
                    if (fuProjectCover.HasFile) coverPath = UploadFile(fuProjectCover, "cover");

                    UpdateProjectInDb(projectId, projectName, description, logoPath, coverPath);
                }
                else
                {
                    // CREATE MODE
                    if (IsProjectNameTaken(userId, projectName))
                    {
                        ShowError("Project name already exists for this user.");
                        KeepModalOpen();
                        return;
                    }

                    string logoPath = "assets/images/projectTestAvatar.jpg";
                    string coverPath = "assets/images/default_cover.jpg";

                    if (fuProjectLogo.HasFile) logoPath = UploadFile(fuProjectLogo, "logo");
                    if (fuProjectCover.HasFile) coverPath = UploadFile(fuProjectCover, "cover");

                    SaveProjectToDb(userId, projectName, description, logoPath, coverPath);
                }
                
                LoadProjects();
                
                // Update Sidebar
                if (Master is SideBar sideBar)
                {
                    sideBar.UpdateProjectList();
                }

                // Reset Form
                txtProjectName.Text = string.Empty;
                txtDescription.Text = string.Empty;
                hfEditProjectId.Value = string.Empty;
                lblModalTitle.Text = "Create Project";
                btnVisibleCreate.InnerText = "Create";
                imgLogoPreview.ImageUrl = "assets/images/projectTestAvatar.jpg";
                imgCoverPreview.ImageUrl = "assets/images/default_cover.jpg";

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "closeProjectModal();", true);
            }
            catch (Exception ex)
            {
                Log($"Error in btnCreateProject_Click: {ex.Message}");
                ShowError("Error: " + ex.Message);
                KeepModalOpen();
            }
        }

        private void UpdateProjectInDb(int projectId, string name, string description, string logo, string cover)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE Projects SET ProjectName=@Name, Description=@Description, ProjectLogo=@Logo, ProjectCover=@Cover WHERE ProjectID=@ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Logo", logo);
                cmd.Parameters.AddWithValue("@Cover", cover);
                cmd.Parameters.AddWithValue("@ID", projectId);
                cmd.ExecuteNonQuery();
            }
        }

        private int GetUserId(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT UserID FROM Users WHERE Username = @Username", conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    object result = cmd.ExecuteScalar();
                    return result != null ? (int)result : -1;
                }
                catch (Exception ex)
                {
                    Log($"Error in GetUserId: {ex.Message}");
                    return -1;
                }
            }
        }

        private bool IsProjectNameTaken(int userId, string projectName, int? excludeProjectId = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Projects WHERE CreatorUserId = @UserId AND ProjectName = @ProjectName";
                    if (excludeProjectId.HasValue)
                    {
                        query += " AND ProjectID != @ExcludeID";
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProjectName", projectName);
                    if (excludeProjectId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@ExcludeID", excludeProjectId.Value);
                    }

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    Log($"Error in IsProjectNameTaken: {ex.Message}");
                    return true; // Fail safe
                }
            }
        }

        private void SaveProjectToDb(int userId, string projectName, string description, string logo, string cover)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // No try-catch here, let it bubble up to btnCreateProject_Click which logs it
                conn.Open();
                string query = @"INSERT INTO Projects (CreatorUserId, ProjectName, Description, CreatedAt, ProjectLogo, ProjectCover) 
                                 VALUES (@UserId, @ProjectName, @Description, @CreatedAt, @ProjectLogo, @ProjectCover)";
                
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProjectName", projectName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@ProjectLogo", logo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ProjectCover", cover ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        private string UploadFile(System.Web.UI.WebControls.FileUpload fileUpload, string type)
        {
            try
            {
                string filename = Path.GetFileName(fileUpload.FileName);
                string extension = Path.GetExtension(filename).ToLower();
                
                // Validate extension
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".gif" && extension != ".svg")
                {
                    Log($"Invalid file extension: {extension}");
                    return type == "logo" ? "assets/images/default-project-logo.png" : "assets/images/default-project-cover.png";
                }

                string uniqueName = $"project_{type}_{DateTime.Now.Ticks}{extension}";
                string saveDir = Server.MapPath("~/assets/uploads/projects/");
                
                if (!Directory.Exists(saveDir))
                    Directory.CreateDirectory(saveDir);

                string savePath = Path.Combine(saveDir, uniqueName);
                fileUpload.SaveAs(savePath);
                
                return "assets/uploads/projects/" + uniqueName;
            }
            catch (Exception ex)
            {
                Log($"Error uploading file: {ex.Message}");
                return type == "logo" ? "assets/images/projectTestAvatar.jpg" : "assets/images/default_cover.jpg";
            }
        }



        private void ShowError(string message)
        {
            lblProjectError.Text = message;
        }

        private void KeepModalOpen()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepModalOpen", "openProjectModal();", true);
        }

        private void LoadProjects()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Fetch projects where user is creator OR project belongs to a team created by user
                    string query = @"
                        SELECT DISTINCT 
                            p.*,
                            CASE 
                                WHEN p.CreatorUserId = @CurrentUserId THEN 1
                                WHEN EXISTS (
                                    SELECT 1 
                                    FROM ProjectTeams pt 
                                    JOIN Team t ON pt.TeamID = t.TeamId
                                    WHERE pt.ProjectID = p.ProjectID 
                                    AND t.CreatorUsername = @CurrentUser
                                ) THEN 1
                                ELSE 0 
                            END as CanEdit
                        FROM Projects p
                        LEFT JOIN ProjectTeams pt ON p.ProjectID = pt.ProjectID
                        LEFT JOIN Team t ON pt.TeamID = t.TeamId
                        WHERE (t.CreatorUsername = @CurrentUser) 
                           OR (p.CreatorUserId = @CurrentUserId)
                        ORDER BY p.CreatedAt DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CurrentUser", Session["User"].ToString());
                    cmd.Parameters.AddWithValue("@CurrentUserId", CurrentUserId);
                    
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

        protected bool IsProjectAdmin(object canEditObj)
        {
            if (canEditObj == DBNull.Value) return false;
            return Convert.ToInt32(canEditObj) == 1;
        }
    }
}