using System;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace TaskQuest
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string input = txtIdentifier.Text.Trim();
            
            if (string.IsNullOrEmpty(input))
            {
                ShowError(identifierError, "Please enter your email or username.");
                return;
            }

            if (UserExists(input))
            {
                ViewState["ResetUser"] = input;
                pnlStep1.Visible = false;
                pnlStep2.Visible = true;
                identifierError.InnerText = ""; // Clear error
            }
            else
            {
                ShowError(identifierError, "You are not our user yet"); // As requested
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            string newPass = txtNewPassword.Text;
            string confirmPass = txtConfirmPassword.Text;
            string userIdentifier = ViewState["ResetUser"] as string;

            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                ShowError(passwordError, "Please fill in all fields.");
                return;
            }

            if (newPass != confirmPass)
            {
                ShowError(passwordError, "Passwords do not match.");
                return;
            }

            if (string.IsNullOrEmpty(userIdentifier))
            {
                // Session expired or something went wrong
                Response.Redirect("ForgotPassword.aspx");
                return;
            }

            if (UpdatePassword(userIdentifier, newPass))
            {
                // Password updated successfully
                // Redirect to Login or show success
                string script = "alert('Password updated successfully!'); window.location='Login.aspx';";
                ClientScript.RegisterStartupScript(this.GetType(), "Success", script, true);
            }
            else
            {
                ShowError(passwordError, "Error updating password. Please try again.");
            }
        }

        private bool UserExists(string input)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Email = @Input OR Username = @Input";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Input", input);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private bool UpdatePassword(string identifier, string newPassword)
        {
            string connectionString = ConnectionHelper.GetConnectionString();
            string hashedPassword = HashPassword(newPassword);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Users SET PasswordHash = @Password WHERE Email = @Input OR Username = @Input";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@Input", identifier);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private void ShowError(System.Web.UI.HtmlControls.HtmlGenericControl errorSpan, string message)
        {
            errorSpan.InnerText = message;
            errorSpan.Attributes["class"] = "error-message show";
            // Optional: Add some script to shake or highlight
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