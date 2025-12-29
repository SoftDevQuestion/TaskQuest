using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Collections.Generic;
using System.IO;

namespace TaskQuest
{
    public partial class Projects : System.Web.UI.Page
    {
        string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LoadProjects();
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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "KeepModalOpen", "openProjectModal();", true);
        }

        private void LoadProjects()
        {
            if (Session["User"] == null) return;

            string username = Session["User"].ToString();
            int userId = GetUserId(username);

            if (userId == -1)
            {
                lblNoProjects.Visible = true;
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Fetch all projects (removed CreatorUserId filter)
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Projects ORDER BY CreatedAt DESC", conn);
                    // cmd.Parameters.AddWithValue("@UserId", userId);

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
                        lblNoProjects.Visible = true;
                        rptProjects.DataSource = null;
                        rptProjects.DataBind();
                    }
                }
                catch
                {
                    lblNoProjects.Visible = true;
                    lblNoProjects.Text = "Error loading projects.";
                }
            }
        }
    }
}
