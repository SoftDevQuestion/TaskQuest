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
                string query = "SELECT FullName, Username, Email, AvatarPath FROM Users WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtFullName.Text = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "";
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
            string newFullName = txtFullName.Text.Trim();
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
                    lblError.Text = "Upload Error: " + ex.Message;
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check Username uniqueness if changed
                    if (!string.Equals(currentUsername, newUsername, StringComparison.OrdinalIgnoreCase))
                    {
                        string checkUser = "SELECT COUNT(*) FROM Users WHERE Username = @NewUsername";
                        SqlCommand cmdCheck = new SqlCommand(checkUser, conn);
                        cmdCheck.Parameters.AddWithValue("@NewUsername", newUsername);
                        int count = (int)cmdCheck.ExecuteScalar();
                        if (count > 0)
                        {
                            lblError.Text = "Username already exists";
                            return;
                        }
                    }

                    // Check Email uniqueness
                    string checkEmail = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Username != @CurrentUsername";
                    SqlCommand cmdCheckEmail = new SqlCommand(checkEmail, conn);
                    cmdCheckEmail.Parameters.AddWithValue("@Email", email);
                    cmdCheckEmail.Parameters.AddWithValue("@CurrentUsername", currentUsername);
                    int emailCount = (int)cmdCheckEmail.ExecuteScalar();
                    if (emailCount > 0)
                    {
                        lblError.Text = "Email already exists";
                        return;
                    }

                    // Build Update Query
                    string query = "UPDATE Users SET FullName = @FullName, Email = @Email, AvatarPath = @AvatarPath";

                    if (!string.Equals(currentUsername, newUsername, StringComparison.OrdinalIgnoreCase))
                    {
                        query += ", Username = @NewUsername";
                    }

                    if (!string.IsNullOrEmpty(password))
                    {
                        query += ", PasswordHash = @PasswordHash";
                    }

                    query += " WHERE Username = @CurrentUsername";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FullName", newFullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@AvatarPath", avatarPath);
                    cmd.Parameters.AddWithValue("@CurrentUsername", currentUsername);

                    if (!string.Equals(currentUsername, newUsername, StringComparison.OrdinalIgnoreCase))
                    {
                        cmd.Parameters.AddWithValue("@NewUsername", newUsername);
                    }

                    if (!string.IsNullOrEmpty(password))
                    {
                        cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(password));
                    }

                    cmd.ExecuteNonQuery();

                    // Update Session if username changed
                    if (!string.Equals(currentUsername, newUsername, StringComparison.OrdinalIgnoreCase))
                    {
                        Session["User"] = newUsername;
                        System.Web.Security.FormsAuthentication.SetAuthCookie(newUsername, false);
                    }
                    
                    // Update Avatar in Session
                    Session["UserAvatar"] = avatarPath;

                    // Clear error
                    lblError.Text = "";

                    // Reload data to reflect changes
                    LoadUserData();
                    
                    // Show success message and redirect/close modal
                    string script = @"
                        var popup = document.getElementById('successPopup');
                        if(popup) {
                            popup.style.display = 'block';
                            setTimeout(function() {
                                if(window.parent && window.parent.closeProfileModal) {
                                    window.parent.closeProfileModal();
                                    window.parent.location.reload(); // Reload parent to update avatar in sidebar
                                } else {
                                    window.location.href = 'Dashboard.aspx';
                                }
                            }, 2000); // Wait 2 seconds before closing
                        }
                    ";
                    ClientScript.RegisterStartupScript(this.GetType(), "SaveSuccess", script, true);
                }
                catch (Exception ex)
                {
                    lblError.Text = "DB Error: " + ex.Message;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Close modal if in modal, else redirect
            string script = @"
                if(window.parent && window.parent.closeProfileModal) {
                    window.parent.closeProfileModal();
                } else {
                    window.location.href = 'Dashboard.aspx';
                }
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "CancelAction", script, true);
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
