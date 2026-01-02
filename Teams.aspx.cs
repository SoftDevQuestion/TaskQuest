using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace TaskQuest
{
    public partial class Teams : System.Web.UI.Page
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
        }

        [WebMethod]
        public static List<UserDTO> SearchUsers(string term)
        {
            List<UserDTO> users = new List<UserDTO>();
            string connStr = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT TOP 5 Username, Email FROM Users WHERE Username LIKE @Term OR Email LIKE @Term";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Term", "%" + term + "%");
                
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new UserDTO
                        {
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
                catch { }
            }
            return users;
        }

        public class UserDTO
        {
            public string Username { get; set; }
            public string Email { get; set; }
        }

        protected void btnTeamUp_Click(object sender, EventArgs e)
        {
            string teamName = hfTeamName.Value;
            string membersJson = hfTeamMembers.Value;

            if (string.IsNullOrEmpty(teamName)) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Create Team
                    string insertTeamQuery = "INSERT INTO teams (name) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdTeam = new SqlCommand(insertTeamQuery, conn, transaction);
                    cmdTeam.Parameters.AddWithValue("@Name", teamName);
                    
                    decimal newTeamIdDec = (decimal)cmdTeam.ExecuteScalar();
                    int newTeamId = Convert.ToInt32(newTeamIdDec);

                    // 2. Add Creator (Current User) as Admin
                    string currentUser = Session["User"].ToString();
                    UpdateUserTeam(conn, transaction, currentUser, newTeamId, "admin");

                    // 3. Add Members
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string[] members = serializer.Deserialize<string[]>(membersJson);

                    if (members != null)
                    {
                        foreach (string memberInput in members)
                        {
                            UpdateUserTeam(conn, transaction, memberInput, newTeamId, "member");
                        }
                    }

                    transaction.Commit();
                    Response.Redirect(Request.RawUrl);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine("Error creating team: " + ex.Message);
                }
            }
        }

        private void UpdateUserTeam(SqlConnection conn, SqlTransaction trans, string userIdentifier, int teamId, string role)
        {
            string findUserQuery = "SELECT Username FROM Users WHERE Username = @Id OR Email = @Id";
            SqlCommand findCmd = new SqlCommand(findUserQuery, conn, trans);
            findCmd.Parameters.AddWithValue("@Id", userIdentifier);
            object result = findCmd.ExecuteScalar();

            if (result != null)
            {
                string username = result.ToString();
                
                string updateQuery = @"
                    UPDATE user_profiles 
                    SET team_id = @TeamId, role = @Role 
                    WHERE username = @Username";
                
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn, trans);
                updateCmd.Parameters.AddWithValue("@TeamId", teamId);
                updateCmd.Parameters.AddWithValue("@Role", role);
                updateCmd.Parameters.AddWithValue("@Username", username);
                updateCmd.ExecuteNonQuery();
            }
        }
    }
}