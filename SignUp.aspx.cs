using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace TaskQuest
{
    public partial class SignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string username = Request.Form["username"];
                string email = Request.Form["email"];
                string password = Request.Form["password"];
                string confirmPassword = Request.Form["confirmPassword"];

                if (password != confirmPassword)
                {
                    ShowError("confirmPassword", "Passwords do not match!");
                    return;
                }

                RegisterUser(username, email, password);
            }
        }

        private void RegisterUser(string username, string email, string password)
        {
            string connectionString = ConnectionHelper.GetConnectionString();
            Log($"Attempting registration for {username} ({email}). Connection string: {connectionString}");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Log("Connection opened successfully.");

                    // Check if Email exists
                    string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, conn);
                    checkEmailCmd.Parameters.AddWithValue("@Email", email);
                    int emailCount = (int)checkEmailCmd.ExecuteScalar();

                    if (emailCount > 0)
                    {
                        ShowError("email", "This email is already registered!");
                        return;
                    }

                    // Check if Username exists
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, conn);
                    checkUserCmd.Parameters.AddWithValue("@Username", username);
                    int userCount = (int)checkUserCmd.ExecuteScalar();

                    if (userCount > 0)
                    {
                        ShowError("username", "This username is already taken");
                        return;
                    }

                    // Hash password (simple SHA256)
                    string passwordHash = HashPassword(password);

                    // Insert User
                    string insertQuery = "INSERT INTO Users (FullName, Username, Email, PasswordHash, CreatedAt) VALUES (@FullName, @Username, @Email, @PasswordHash, @CreatedAt)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@FullName", username); // Use username as FullName for now
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    insertCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    insertCmd.ExecuteNonQuery();

                    // Auto-login after successful signup
                    Session["User"] = username;
                    FormsAuthentication.SetAuthCookie(username, false);
                    
                    // Redirect to avatar selection
                    Response.Redirect("ChooseAvatar.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    // Generic error (shown on username field for simplicity or specific generic error container if existed)
                    // Trying to parse common SQL errors
                    string msg = ex.Message.Replace("'", "\\'");
                    ShowError("username", "System Error: " + msg);
                    Log("Error in RegisterUser: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
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
    }
}
