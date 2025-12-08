using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace TaskQuest
{
    public partial class Profile : BasePage
    {
        string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;

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
                // Load Default Avatars
                rptAvatars.DataSource = Enumerable.Range(1, 9)
                    .Select(i => $"assets/images/avatar{i}.png");
                rptAvatars.DataBind();

                LoadUserData();
            }
        }

        private void LoadUserData()
        {
            string username = Session["User"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Username, Email, AvatarPath FROM Users WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtUser.Text = reader["Username"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        string avatar = reader["AvatarPath"] != DBNull.Value ? reader["AvatarPath"].ToString() : "";

                        if (!string.IsNullOrEmpty(avatar))
                        {
                            imgAvatarPreview.ImageUrl = avatar;
                            hdSelectedAvatar.Value = avatar;
                        }
                        else
                        {
                            imgAvatarPreview.ImageUrl = "assets/images/avatar1.png";
                            hdSelectedAvatar.Value = "assets/images/avatar1.png";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle error (log it)
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["User"] == null) return;

            string currentUsername = Session["User"].ToString();
            string newUsername = txtUser.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPass.Text;
            string avatarPath = hdSelectedAvatar.Value;

            // Handle File Upload
            if (fuAvatar.HasFile)
            {
                try
                {
                    string filename = Path.GetFileName(fuAvatar.FileName);
                    string extension = Path.GetExtension(filename);
                    string uniqueName = "avatar_" + DateTime.Now.Ticks + extension;
                    string saveDir = Server.MapPath("~/assets/uploads/");
                    
                    if (!Directory.Exists(saveDir))
                        Directory.CreateDirectory(saveDir);

                    string savePath = Path.Combine(saveDir, uniqueName);
                    fuAvatar.SaveAs(savePath);
                    
                    // Update avatar path to be relative
                    avatarPath = "assets/uploads/" + uniqueName;
                }
                catch (Exception ex)
                {
                    // Handle upload error
                    System.Diagnostics.Debug.WriteLine("Upload Error: " + ex.Message);
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Build Update Query
                    string query = "UPDATE Users SET Email = @Email, AvatarPath = @AvatarPath";

                    if (!string.IsNullOrEmpty(newUsername) && newUsername != currentUsername)
                    {
                        query += ", Username = @NewUsername";
                    }

                    if (!string.IsNullOrEmpty(password))
                    {
                        query += ", PasswordHash = @PasswordHash";
                    }

                    query += " WHERE Username = @CurrentUsername";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@AvatarPath", avatarPath);
                    cmd.Parameters.AddWithValue("@CurrentUsername", currentUsername);

                    if (!string.IsNullOrEmpty(newUsername) && newUsername != currentUsername)
                    {
                        cmd.Parameters.AddWithValue("@NewUsername", newUsername);
                    }

                    if (!string.IsNullOrEmpty(password))
                    {
                        cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(password));
                    }

                    cmd.ExecuteNonQuery();

                    // Update Session if username changed
                    if (!string.IsNullOrEmpty(newUsername) && newUsername != currentUsername)
                    {
                        Session["User"] = newUsername;
                        System.Web.Security.FormsAuthentication.SetAuthCookie(newUsername, false);
                    }
                    
                    // Update Avatar in Session
                    Session["UserAvatar"] = avatarPath;

                    // Reload data to reflect changes
                    LoadUserData();
                    
                    // Show success message or redirect
                    // For now, reload the page content is enough as requested
                }
                catch (Exception ex)
                {
                    // Handle DB error
                    System.Diagnostics.Debug.WriteLine("DB Error: " + ex.Message);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
