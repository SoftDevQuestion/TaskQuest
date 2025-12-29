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

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            if (Session["User"] == null) return;

            string username = Session["User"].ToString();
            string projectName = txtProjectName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(projectName))
            {
                ShowError("Project Name is required");
                KeepModalOpen();
                return;
            }

            int userId = GetUserId(username);
            if (userId == -1)
            {
                ShowError("User not found");
                return;
            }

            // Check Uniqueness
            if (IsProjectNameTaken(userId, projectName))
            {
                ShowError("Project name already exists for this user.");
                KeepModalOpen();
                return;
            }

            // Handle Files
            string logoPath = "assets/images/default-project-logo.png";
            string coverPath = "assets/images/default-project-cover.png";

            if (fuProjectLogo.HasFile)
            {
                logoPath = UploadFile(fuProjectLogo, "logo");
            }

            if (fuProjectCover.HasFile)
            {
                coverPath = UploadFile(fuProjectCover, "cover");
            }

            // Save to DB
            SaveProjectToDb(userId, projectName, description, logoPath, coverPath);

            // Refresh and Close
            txtProjectName.Text = "";
            txtDescription.Text = "a brief of how i wanna change the world!";
            Response.Redirect(Request.RawUrl);
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
                catch
                {
                    return -1;
                }
            }
        }

        private bool IsProjectNameTaken(int userId, string projectName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Projects WHERE CreatorUserId = @UserId AND ProjectName = @ProjectName", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProjectName", projectName);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private string UploadFile(System.Web.UI.WebControls.FileUpload fileUpload, string type)
        {
            try
            {
                string filename = Path.GetFileName(fileUpload.FileName);
                string extension = Path.GetExtension(filename);
                string uniqueName = $"project_{type}_{DateTime.Now.Ticks}{extension}";
                string saveDir = Server.MapPath("~/assets/uploads/projects/");
                
                if (!Directory.Exists(saveDir))
                    Directory.CreateDirectory(saveDir);

                string savePath = Path.Combine(saveDir, uniqueName);
                fileUpload.SaveAs(savePath);
                
                return "assets/uploads/projects/" + uniqueName;
            }
            catch
            {
                return type == "logo" ? "assets/images/default-project-logo.png" : "assets/images/default-project-cover.png";
            }
        }

        private void SaveProjectToDb(int userId, string projectName, string description, string logo, string cover)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Projects (CreatorUserId, ProjectName, Description, CreatedAt, ProjectLogo, ProjectCover) 
                                 VALUES (@UserId, @ProjectName, @Description, @CreatedAt, @ProjectLogo, @ProjectCover)";
                
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProjectName", projectName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@ProjectLogo", logo);
                cmd.Parameters.AddWithValue("@ProjectCover", cover);

                cmd.ExecuteNonQuery();
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Fetch projects created by this user
                SqlCommand cmd = new SqlCommand("SELECT * FROM Projects WHERE CreatorUserId = @UserId ORDER BY CreatedAt DESC", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptProjects.DataSource = dt;
                rptProjects.DataBind();
            }
        }
    }
}