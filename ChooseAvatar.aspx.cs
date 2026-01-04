using System;
using System.IO;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;

namespace TaskQuest
{
    public partial class ChooseAvatar : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadUserInfo();
            }
        }

        private void LoadUserInfo()
        {
            string username = Session["User"].ToString();
            
            // Set Persian text with proper encoding
            SetControlText(lblHeader, "Complete Your Profile");
            SetControlText(lblSubHeader, "Please select a profile picture");
            SetControlText(lblDefaultAvatars, "Preset Avatars");
            SetControlText(lblUploadTitle, "Or upload your own photo");
            
            // Set button texts
            skipButton.Text = SafeEncodePersianText("Skip for now");
            completeButton.Text = SafeEncodePersianText("Confirm");
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visible = true;
            // Ensure proper encoding for error messages
            ErrorMessage.Attributes.Add("style", "direction: rtl; text-align: right;");
        }

        protected void SkipButton_Click(object sender, EventArgs e)
        {
            // Skip avatar selection and go to dashboard
            Response.Redirect("Dashboard.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void CompleteButton_Click(object sender, EventArgs e)
        {
            try 
            {
                if (Session["User"] != null)
                {
                    string username = Session["User"].ToString();
                    string avatarPath = selectedAvatarPath.Value;

                    // Check for file upload (only if no default avatar is currently selected)
                    if (string.IsNullOrEmpty(avatarPath) && fileUpload.HasFile)
                    {
                        try
                        {
                            string filename = Path.GetFileName(fileUpload.FileName);
                            string extension = Path.GetExtension(filename).ToLower();
                            
                            // Validate extension
                            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                            {
                                string uniqueName = $"avatar_{DateTime.Now.Ticks}{extension}";
                                string saveDir = Server.MapPath("~/assets/uploads/avatars/");
                                
                                if (!Directory.Exists(saveDir))
                                    Directory.CreateDirectory(saveDir);

                                string savePath = Path.Combine(saveDir, uniqueName);
                                fileUpload.SaveAs(savePath);
                                
                                avatarPath = "assets/uploads/avatars/" + uniqueName;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error uploading file: " + ex.Message);
                        }
                    }

                    if (string.IsNullOrEmpty(avatarPath))
                    {
                        // If no avatar selected, use default
                        avatarPath = "assets/images/avatar1.png";
                    }

                    // Save avatar path to database
                    string connectionString = ConnectionHelper.GetConnectionString();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        
                        // Update user avatar
                        string updateQuery = "UPDATE Users SET AvatarPath = @AvatarPath WHERE Username = @Username";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@AvatarPath", avatarPath);
                        updateCmd.Parameters.AddWithValue("@Username", username);
                        
                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            // Save avatar path in session for immediate use
                            Session["UserAvatar"] = avatarPath;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but continue to redirect
                System.Diagnostics.Debug.WriteLine("Error in ChooseAvatar: " + ex.Message);
            }

            // Redirect to dashboard
            Response.Redirect("Dashboard.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}