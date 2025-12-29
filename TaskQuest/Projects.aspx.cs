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

                Log($"User: {username}, Project: {projectName}");

                if (string.IsNullOrEmpty(projectName))
                {
                    Log("Project name is empty.");
                    ShowError("Project Name is required");
                    KeepModalOpen();
                    return;
                }

                int userId = GetUserId(username);
                Log($"UserID: {userId}");

                if (userId == -1)
                {
                    Log("User not found.");
                    ShowError("User not found in database. Please re-login.");
                    KeepModalOpen();
                    return;
                }

                // Check Uniqueness
                if (IsProjectNameTaken(userId, projectName))
                {
                    Log("Project name taken.");
                    ShowError("Project name already exists for this user.");
                    KeepModalOpen();
                    return;
                }

                // Handle Files
                string logoPath = "assets/images/projectTestAvatar.jpg";
                string coverPath = "assets/images/default_cover.jpg";

                if (fuProjectLogo.HasFile)
                {
                    Log("Uploading logo...");
                    logoPath = UploadFile(fuProjectLogo, "logo");
                }

                if (fuProjectCover.HasFile)
                {
                    Log("Uploading cover...");
                    coverPath = UploadFile(fuProjectCover, "cover");
                }

                Log($"Saving to DB. Logo: {logoPath}, Cover: {coverPath}");

                // Save to DB
                SaveProjectToDb(userId, projectName, description, logoPath, coverPath);
                
                Log("Save successful. Refreshing grid.");

                // Refresh Grid and Close Modal locally (avoids Redirect issues)
                LoadProjects();
                
                // Clear inputs
                txtProjectName.Text = string.Empty;
                txtDescription.Text = string.Empty;
                
                // Close modal
                ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", "closeProjectModal();", true);
            }
            catch (Exception ex)
            {
                Log($"Error in btnCreateProject_Click: {ex.Message}\nStack: {ex.StackTrace}");
                ShowError("Error: " + ex.Message);
                KeepModalOpen();
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

        private bool IsProjectNameTaken(int userId, string projectName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Projects WHERE CreatorUserId = @UserId AND ProjectName = @ProjectName", conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProjectName", projectName);
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
            ClientScript.RegisterStartupScript(this.GetType(), "KeepModalOpen", "openProjectModal();", true);
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