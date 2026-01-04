using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Web.Script.Serialization;

namespace TaskQuest
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                // Handle Google login response
                if (!string.IsNullOrEmpty(Request.Form["credential"]))
                {
                    HandleGoogleLogin(Request.Form["credential"]);
                }
                else
                {
                    string input = Request.Form["email"];
                    string password = Request.Form["password"];

                    LoginUser(input, password);
                }
            }
        }

        private void LoginUser(string input, string password)
        {
            string connectionString = ConnectionHelper.GetConnectionString();
            Log($"Attempting login for {input}. Connection string: {connectionString}");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Log("Connection opened successfully.");

                    // Get user by email or username
                    string query = "SELECT Username, PasswordHash, AvatarPath FROM Users WHERE Email = @Input OR Username = @Input";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Input", input);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string dbPasswordHash = reader["PasswordHash"].ToString();
                        string username = reader["Username"].ToString();
                        string avatarPath = reader["AvatarPath"] != DBNull.Value ? reader["AvatarPath"].ToString() : "";

                        // Check password
                        if (HashPassword(password) == dbPasswordHash)
                        {
                            // Login successful
                            // Set cookie or session
                            Session["User"] = username;
                            if (!string.IsNullOrEmpty(avatarPath))
                            {
                                Session["UserAvatar"] = avatarPath;
                            }
                            FormsAuthentication.SetAuthCookie(username, false);
                            Response.Redirect("Dashboard.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            ShowError("password", "Password incorrect");
                        }
                    }
                    else
                    {
                        ShowError("email", "You are not our user yet");
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.Replace("'", "\\'");
                    ShowError("email", "System Error: " + msg);
                    Log("Error in LoginUser: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
                }
            }
        }

        private void Log(string message)
        {
            try
            {
                string path = Server.MapPath("~/App_Data/auth_debug.log");
                string logMessage = $"{DateTime.Now}: {message}{Environment.NewLine}";
                System.IO.File.AppendAllText(path, logMessage);
            }
            catch { }
        }

        private void ShowError(string field, string message)
        {
            string script = $@"
                document.addEventListener('DOMContentLoaded', function() {{
                    const formGroup = document.getElementById('{field}').closest('.form-group');
                    const errorElement = document.getElementById('{field}Error');
                    if(formGroup && errorElement) {{
                        formGroup.classList.add('error');
                        errorElement.textContent = '{message}';
                        errorElement.classList.add('show');
                        
                        // Add shake animation
                        const input = document.getElementById('{field}');
                        if(input) {{
                            input.style.animation = 'materialShake 0.4s ease-in-out';
                            setTimeout(() => {{ input.style.animation = ''; }}, 400);
                        }}
                    }}
                }});";
            ClientScript.RegisterStartupScript(this.GetType(), "ServerError_" + field, script, true);
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

        private void HandleGoogleLogin(string credential)
        {
            try
            {
                // For now, we'll implement a basic version without JWT parsing
                // In production, you should verify the Google JWT token properly
                
                // Extract basic info from the credential (simplified approach)
                // Note: In production, you should properly decode and verify the JWT token
                string email = "";
                string name = "";
                string googleId = "";
                
                // Basic JWT parsing (base64 decode of payload)
                var parts = credential.Split('.');
                if (parts.Length == 3)
                {
                    try
                    {
                        var payload = parts[1];
                        // Add padding if needed
                        payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
                        var jsonPayload = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                        var serializer = new JavaScriptSerializer();
                        var payloadData = serializer.Deserialize<Dictionary<string, object>>(jsonPayload);
                        
                        if (payloadData.ContainsKey("email")) email = payloadData["email"].ToString();
                        if (payloadData.ContainsKey("name")) name = payloadData["name"].ToString();
                        if (payloadData.ContainsKey("sub")) googleId = payloadData["sub"].ToString();
                    }
                    catch (Exception decodeEx)
                    {
                        Log($"JWT decode error: {decodeEx.Message}");
                    }
                }

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
                {
                    Log($"Google login attempt for email: {email}, name: {name}");
                    
                    // Check if user exists in database
                    string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;
                    
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        
                        // Check if user exists by email
                        string checkQuery = "SELECT Username FROM Users WHERE Email = @Email";
                        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        
                        string username = checkCmd.ExecuteScalar()?.ToString();
                        
                        if (string.IsNullOrEmpty(username))
                        {
                            // Create new user from Google account
                            username = name.Replace(" ", "").ToLower() + (googleId.Length >= 6 ? googleId.Substring(0, 6) : googleId);
                            
                            // Ensure username is unique
                            string uniqueUsername = username;
                            int counter = 1;
                            while (true)
                            {
                                string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                                SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, conn);
                                checkUserCmd.Parameters.AddWithValue("@Username", uniqueUsername);
                                int userCount = (int)checkUserCmd.ExecuteScalar();
                                
                                if (userCount == 0) break;
                                
                                uniqueUsername = username + counter;
                                counter++;
                            }
                            
                            // Create user with Google info (no password needed)
                            string insertQuery = "INSERT INTO Users (FullName, Username, Email, PasswordHash, CreatedAt) VALUES (@FullName, @Username, @Email, @PasswordHash, @CreatedAt)";
                            SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                            insertCmd.Parameters.AddWithValue("@FullName", name);
                            insertCmd.Parameters.AddWithValue("@Username", uniqueUsername);
                            insertCmd.Parameters.AddWithValue("@Email", email);
                            insertCmd.Parameters.AddWithValue("@PasswordHash", "GOOGLE_AUTH"); // Special marker for Google auth
                            insertCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                            
                            insertCmd.ExecuteNonQuery();
                            
                            username = uniqueUsername;
                            Log($"New Google user created: {username}");
                        }
                        
                        // Login successful - set session and cookie
                        Session["User"] = username;
                        FormsAuthentication.SetAuthCookie(username, false);
                        
                        // Redirect to avatar selection page
                        Response.Redirect("ChooseAvatar.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
                else
                {
                    ShowError("email", "خطا در دریافت اطلاعات از گوگل");
                }
            }
            catch (Exception ex)
            {
                Log($"Error in Google login: {ex.Message}");
                ShowError("email", "خطا در ورود با گوگل. لطفا دوباره تلاش کنید.");
            }
        }
    }
}
