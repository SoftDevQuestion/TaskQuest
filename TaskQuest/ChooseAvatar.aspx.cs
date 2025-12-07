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
            SetControlText(lblHeader, "تکمیل پروفایل");
            SetControlText(lblSubHeader, "لطفاً عکس پروفایل خود را انتخاب کنید");
            SetControlText(lblDefaultAvatars, "عکس‌های پیش‌فرض");
            SetControlText(lblUploadTitle, "یا عکس خود را آپلود کنید");
            
            // Set button texts
            skipButton.Text = SafeEncodePersianText("فعلاً از این مرحله بگذر");
            completeButton.Text = SafeEncodePersianText("انجام شد");
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
            Response.Redirect("Dashboard.aspx");
        }

        protected void CompleteButton_Click(object sender, EventArgs e)
        {
            string username = Session["User"].ToString();
            string avatarPath = selectedAvatarPath.Value;

            if (string.IsNullOrEmpty(avatarPath))
            {
                // If no avatar selected, use default
                avatarPath = "assets/images/default-avatar.svg";
            }

            try
            {
                // Save avatar path to database
                string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Check if user has avatar column, if not create it
                    string checkColumnQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.columns 
                        WHERE object_id = OBJECT_ID(N'Users') AND name = 'AvatarPath')
                        BEGIN
                            ALTER TABLE Users ADD AvatarPath NVARCHAR(MAX)
                        END";
                    
                    SqlCommand checkCmd = new SqlCommand(checkColumnQuery, conn);
                    checkCmd.ExecuteNonQuery();

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
            catch (Exception)
            {
                // Ignore errors to ensure redirection happens
            }

            // Redirect to dashboard
            Response.Redirect("Dashboard.aspx");
        }
    }
}