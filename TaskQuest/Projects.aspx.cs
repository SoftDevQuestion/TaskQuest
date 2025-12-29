using System;
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
            SaveProjectToDb(userId, projectName, logoPath, coverPath);

            // Refresh and Close
            txtProjectName.Text = "";
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

        private void SaveProjectToDb(int userId, string projectName, string logo, string cover)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Projects (CreatorUserId, ProjectName, CreatedAt, ProjectLogo, ProjectCover) 
                                 VALUES (@UserId, @ProjectName, @CreatedAt, @ProjectLogo, @ProjectCover)";
                
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProjectName", projectName);
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
            var projects = new List<ProjectVM>
    {
        new ProjectVM
        {
            Title = "Physics",
            Description = "Small and concise headline for physics project",
            ImageUrl = "assets/images/project1.jpeg"
        },
        new ProjectVM
        {
            Title = "UI/UX Design System",
            Description = "Longer description that should not break the layout",
            ImageUrl = "assets/images/project2.jpeg"
        }
    };

            rptProjects.DataSource = projects;
            rptProjects.DataBind();
        }

        public class ProjectVM
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
        }


    }
}