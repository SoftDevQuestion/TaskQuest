using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;

namespace TaskQuest
{
    public partial class Teams : System.Web.UI.Page
    {
        private string connectionString = ConnectionHelper.GetConnectionString();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure form allows file uploads
            this.Form.Enctype = "multipart/form-data";

            // Check if user is logged in
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                hfCurrentUser.Value = Session["User"].ToString();
                LoadTeams();
            }
        }

        private void LoadTeams()
        {
            List<TeamViewModel> teams = new List<TeamViewModel>();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try 
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            t.TeamId, 
                            t.TeamName, 
                            t.Description, 
                            t.LogoPath,
                            t.CreatorUsername,
                            tm.Username, 
                            tm.Role, 
                            u.AvatarPath 
                        FROM Team t 
                        LEFT JOIN TeamMembers tm ON t.TeamId = tm.TeamId 
                        LEFT JOIN Users u ON tm.Username = u.Username
                        WHERE t.TeamId IN (
                            SELECT DISTINCT t1.TeamId 
                            FROM Team t1
                            LEFT JOIN TeamMembers tm1 ON t1.TeamId = tm1.TeamId
                            WHERE t1.CreatorUsername = @CurrentUser OR tm1.Username = @CurrentUser
                        )
                        ORDER BY t.TeamId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CurrentUser", Session["User"].ToString());
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    var grouped = dt.AsEnumerable()
                        .GroupBy(row => row["TeamId"]);

                    foreach (var grp in grouped)
                    {
                        var firstRow = grp.First();
                        TeamViewModel team = new TeamViewModel
                        {
                            TeamId = Convert.ToInt32(firstRow["TeamId"]),
                            TeamName = firstRow["TeamName"].ToString(),
                            Description = firstRow["Description"] != DBNull.Value ? firstRow["Description"].ToString() : "",
                            LogoPath = firstRow["LogoPath"] != DBNull.Value ? firstRow["LogoPath"].ToString() : null,
                            CreatorUsername = firstRow["CreatorUsername"] != DBNull.Value ? firstRow["CreatorUsername"].ToString() : "",
                            Members = new List<MemberViewModel>()
                        };

                        foreach (var row in grp)
                        {
                            if (row["username"] != DBNull.Value)
                            {
                                team.Members.Add(new MemberViewModel
                                {
                                    Username = row["username"].ToString(),
                                    Role = row["role"].ToString(),
                                    AvatarPath = row["AvatarPath"] != DBNull.Value && !string.IsNullOrEmpty(row["AvatarPath"].ToString()) 
                                        ? row["AvatarPath"].ToString() 
                                        : "assets/images/default-avatar.svg"
                                });
                            }
                        }
                        
                        teams.Add(team);
                    }

                    // Set IsAdmin for each team
                    string currentUser = Session["User"].ToString();
                    foreach (var team in teams)
                    {
                        team.IsAdmin = (currentUser == team.CreatorUsername) || 
                                       team.Members.Any(m => m.Username == currentUser && m.Role == "admin");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error loading teams: " + ex.Message);
                }
            }

            rptTeams.DataSource = teams;
            rptTeams.DataBind();
        }

        protected void rptTeams_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int teamId = Convert.ToInt32(e.CommandArgument);
                DeleteTeam(teamId);
                LoadTeams();
            }
            else if (e.CommandName == "Edit")
            {
                int teamId = Convert.ToInt32(e.CommandArgument);
                hfEditTeamId.Value = teamId.ToString();
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT TeamName, Description, LogoPath FROM Team WHERE TeamId = @TeamId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TeamId", teamId);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtEditTeamName.Text = reader["TeamName"].ToString();
                            txtEditTeamDescription.Text = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
                            string logo = reader["LogoPath"] as string;

                            if (!string.IsNullOrEmpty(logo))
                            {
                                // Show real logo, hide preview icon and placeholder text
                                imgEditLogoReal.ImageUrl = logo;
                                imgEditLogoReal.Style["display"] = "block";
                                imgEditLogoPreview.Style["display"] = "none";
                                editLogoPlaceholderText.Style["display"] = "none";
                            }
                            else
                            {
                                // Show preview icon (plus) and placeholder text
                                imgEditLogoPreview.ImageUrl = "assets/images/plus-icon.svg";
                                imgEditLogoPreview.Style["display"] = "block";
                                imgEditLogoReal.Style["display"] = "none";
                                editLogoPlaceholderText.Style["display"] = "block";
                            }
                        }
                        reader.Close();
                    }

                    // Fetch Members with Avatar
                    List<MemberDTO> members = new List<MemberDTO>();
                    string memberQuery = @"
                        SELECT tm.Username, tm.Role, u.AvatarPath 
                        FROM TeamMembers tm
                        LEFT JOIN Users u ON tm.Username = u.Username
                        WHERE tm.TeamId = @TeamId";
                    
                    SqlCommand memberCmd = new SqlCommand(memberQuery, conn);
                    memberCmd.Parameters.AddWithValue("@TeamId", teamId);
                    using (SqlDataReader memberReader = memberCmd.ExecuteReader())
                    {
                        while (memberReader.Read())
                        {
                            members.Add(new MemberDTO
                            {
                                username = memberReader["Username"].ToString(),
                                role = memberReader["Role"].ToString(),
                                avatar = memberReader["AvatarPath"] != DBNull.Value ? memberReader["AvatarPath"].ToString() : null
                            });
                        }
                    }
                    
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    hfEditTeamMembersInitial.Value = serializer.Serialize(members);

                    // Show Modal
                    ClientScript.RegisterStartupScript(this.GetType(), "Pop", "showEditTeamModal();", true);
                }
            }
        }

        private bool IsTeamAdmin(int teamId, string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = @"
                        SELECT COUNT(*) 
                        FROM Team t 
                        LEFT JOIN TeamMembers tm ON t.TeamId = tm.TeamId 
                        WHERE t.TeamId = @TeamId 
                        AND (t.CreatorUsername = @Username OR (tm.Username = @Username AND tm.Role = 'admin'))";
                    
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@TeamId", teamId);
                    cmd.Parameters.AddWithValue("@Username", username);
                    
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            int teamId;
            if (int.TryParse(hfEditTeamId.Value, out teamId))
            {
                if (!IsTeamAdmin(teamId, Session["User"].ToString())) return;

                string newName = txtEditTeamName.Text.Trim();
                string newDescription = txtEditTeamDescription.Text.Trim();
                if (string.IsNullOrEmpty(newName)) return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check for duplicate name
                    string checkTeamQuery = "SELECT COUNT(*) FROM Team WHERE TeamName = @Name AND TeamId != @Id";
                    SqlCommand checkCmd = new SqlCommand(checkTeamQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Name", newName);
                    checkCmd.Parameters.AddWithValue("@Id", teamId);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "showEditTeamModal(); document.getElementById('lblEditTeamError').innerText = 'Team name already exists. Please choose another name.'; document.getElementById('lblEditTeamError').style.display = 'block';", true);
                        return;
                    }

                    // Update Team Info
                    string logoPath = null;
                    if (fuEditTeamLogo.HasFile)
                    {
                        try
                        {
                            string filename = System.IO.Path.GetFileName(fuEditTeamLogo.FileName);
                            string extension = System.IO.Path.GetExtension(filename);
                            string uniqueName = Guid.NewGuid().ToString() + extension;
                            string folderPath = Server.MapPath("~/assets/images/teams/");
                            if (!System.IO.Directory.Exists(folderPath))
                            {
                                System.IO.Directory.CreateDirectory(folderPath);
                            }
                            string savePath = System.IO.Path.Combine(folderPath, uniqueName);
                            fuEditTeamLogo.SaveAs(savePath);
                            logoPath = "assets/images/teams/" + uniqueName;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Upload error: " + ex.Message);
                        }
                    }

                    if (logoPath != null)
                    {
                         string query = "UPDATE Team SET TeamName = @TeamName, Description = @Description, UpdatedAt = GETDATE(), LogoPath = @LogoPath WHERE TeamId = @TeamId";
                         SqlCommand cmd = new SqlCommand(query, conn);
                         cmd.Parameters.AddWithValue("@TeamName", newName);
                         cmd.Parameters.AddWithValue("@Description", newDescription);
                         cmd.Parameters.AddWithValue("@TeamId", teamId);
                         cmd.Parameters.AddWithValue("@LogoPath", logoPath);
                         cmd.ExecuteNonQuery();
                    }
                    else
                    {
                         string query = "UPDATE Team SET TeamName = @TeamName, Description = @Description, UpdatedAt = GETDATE() WHERE TeamId = @TeamId";
                         SqlCommand cmd = new SqlCommand(query, conn);
                         cmd.Parameters.AddWithValue("@TeamName", newName);
                         cmd.Parameters.AddWithValue("@Description", newDescription);
                         cmd.Parameters.AddWithValue("@TeamId", teamId);
                         cmd.ExecuteNonQuery();
                    }

                    // Update Sidebar
                    ((SideBar)Master).LoadRecentTeams();

                    // Update Members
                    string membersJson = hfEditTeamMembers.Value;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    MemberDTO[] newMembers = serializer.Deserialize<MemberDTO[]>(membersJson);

                    if (newMembers != null)
                    {
                        // 1. Get current DB members to identify removed ones
                        List<string> currentDbUsers = new List<string>();
                        string getMembersQuery = "SELECT Username FROM TeamMembers WHERE TeamId = @TeamId";
                        SqlCommand getMembersCmd = new SqlCommand(getMembersQuery, conn);
                        getMembersCmd.Parameters.AddWithValue("@TeamId", teamId);
                        using (SqlDataReader rdr = getMembersCmd.ExecuteReader())
                        {
                            while (rdr.Read()) currentDbUsers.Add(rdr["Username"].ToString());
                        }

                        // 2. Identify and Delete Removed Members
                        // Keep only members present in the new list
                        List<string> newMemberUsernames = new List<string>();
                        foreach (var m in newMembers) newMemberUsernames.Add(m.username);

                        foreach (string dbUser in currentDbUsers)
                        {
                            if (!newMemberUsernames.Contains(dbUser))
                            {
                                string deleteMemberQuery = "DELETE FROM TeamMembers WHERE TeamId = @TeamId AND Username = @Username";
                                SqlCommand deleteCmd = new SqlCommand(deleteMemberQuery, conn);
                                deleteCmd.Parameters.AddWithValue("@TeamId", teamId);
                                deleteCmd.Parameters.AddWithValue("@Username", dbUser);
                                deleteCmd.ExecuteNonQuery();
                            }
                        }

                        // 3. Add or Update Members
                        foreach (MemberDTO member in newMembers)
                        {
                            // Using UpdateUserTeam which handles Insert/Check logic
                            // But UpdateUserTeam currently only Inserts if not exists.
                            // We need to UPDATE role if exists.
                            
                            // Check if exists
                            string checkMemberQuery = "SELECT COUNT(*) FROM TeamMembers WHERE TeamId = @TeamId AND Username = @Username";
                            SqlCommand checkMemberCmd = new SqlCommand(checkMemberQuery, conn);
                            checkMemberCmd.Parameters.AddWithValue("@TeamId", teamId);
                            checkMemberCmd.Parameters.AddWithValue("@Username", member.username);
                            int exists = (int)checkMemberCmd.ExecuteScalar();

                            if (exists > 0)
                            {
                                // Update Role
                                string updateRoleQuery = "UPDATE TeamMembers SET Role = @Role WHERE TeamId = @TeamId AND Username = @Username";
                                SqlCommand updateRoleCmd = new SqlCommand(updateRoleQuery, conn);
                                updateRoleCmd.Parameters.AddWithValue("@Role", member.role);
                                updateRoleCmd.Parameters.AddWithValue("@TeamId", teamId);
                                updateRoleCmd.Parameters.AddWithValue("@Username", member.username);
                                updateRoleCmd.ExecuteNonQuery();
                            }
                            else
                            {
                                // Insert (Using helper or direct insert)
                                // Note: Helper fetches user ID again, let's just do it directly if we trust username is valid from client/search
                                // But better to verify user exists in Users table first.
                                string verifyUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username OR Email = @Username";
                                SqlCommand verifyCmd = new SqlCommand(verifyUserQuery, conn);
                                verifyCmd.Parameters.AddWithValue("@Username", member.username);
                                int userExists = (int)verifyCmd.ExecuteScalar();
                                
                                if (userExists > 0)
                                {
                                    // Get real username in case email was used
                                    string getRealUserQuery = "SELECT Username FROM Users WHERE Username = @Username OR Email = @Username";
                                    SqlCommand getRealUserCmd = new SqlCommand(getRealUserQuery, conn);
                                    getRealUserCmd.Parameters.AddWithValue("@Username", member.username);
                                    string realUsername = (string)getRealUserCmd.ExecuteScalar();

                                    string insertQuery = "INSERT INTO TeamMembers (TeamId, Username, Role) VALUES (@TeamId, @Username, @Role)";
                                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                                    insertCmd.Parameters.AddWithValue("@TeamId", teamId);
                                    insertCmd.Parameters.AddWithValue("@Username", realUsername);
                                    insertCmd.Parameters.AddWithValue("@Role", member.role);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            LoadTeams();
        }

        private void DeleteTeam(int teamId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    
                    // 1. Unlink Projects (Set TeamAccessId to NULL) to avoid FK conflict
                    string unlinkProjects = "UPDATE Projects SET TeamAccessId = NULL WHERE TeamAccessId = @TeamId";
                    SqlCommand cmdUnlink = new SqlCommand(unlinkProjects, conn);
                    cmdUnlink.Parameters.AddWithValue("@TeamId", teamId);
                    cmdUnlink.ExecuteNonQuery();

                    // 2. Delete associated members
                    string deleteMembers = "DELETE FROM TeamMembers WHERE TeamId = @TeamId";
                    SqlCommand cmdDeleteMembers = new SqlCommand(deleteMembers, conn);
                    cmdDeleteMembers.Parameters.AddWithValue("@TeamId", teamId);
                    cmdDeleteMembers.ExecuteNonQuery();

                    // 3. Delete team
                    string deleteTeam = "DELETE FROM Team WHERE TeamId = @TeamId";
                    SqlCommand cmdDelete = new SqlCommand(deleteTeam, conn);
                    cmdDelete.Parameters.AddWithValue("@TeamId", teamId);
                    cmdDelete.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error deleting team: " + ex.Message;
                    lblError.Visible = true;
                    System.Diagnostics.Debug.WriteLine("Error deleting team: " + ex.Message);
                }
            }
        }

        private bool IsTeamAdmin(int teamId, string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT COUNT(*) 
                    FROM Team t 
                    LEFT JOIN TeamMembers tm ON t.TeamId = tm.TeamId 
                    WHERE t.TeamId = @TeamId 
                    AND (t.CreatorUsername = @Username OR (tm.Username = @Username AND tm.Role = 'admin'))";
                
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TeamId", teamId);
                cmd.Parameters.AddWithValue("@Username", username);
                
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public class TeamViewModel
        {
            public int TeamId { get; set; }
            public string TeamName { get; set; }
            public string Description { get; set; }
            public string LogoPath { get; set; }
            public string CreatorUsername { get; set; }
            public List<MemberViewModel> Members { get; set; }
            public int MemberCount => Members.Count;
            public bool IsAdmin { get; set; }
        }

        public class MemberViewModel
        {
            public string Username { get; set; }
            public string Role { get; set; }
            public string AvatarPath { get; set; }
        }

        [WebMethod]
        public static List<UserDTO> SearchUsers(string term)
        {
            List<UserDTO> users = new List<UserDTO>();
            string connStr = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT TOP 5 Username, Email, AvatarPath FROM Users WHERE Username LIKE @Term OR Email LIKE @Term";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Term", "%" + term + "%");
                
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string avatar = reader["AvatarPath"] != DBNull.Value && !string.IsNullOrEmpty(reader["AvatarPath"].ToString()) 
                                        ? reader["AvatarPath"].ToString() 
                                        : "assets/images/default-avatar.svg";

                        users.Add(new UserDTO
                        {
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            AvatarPath = avatar
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
            public string AvatarPath { get; set; }
        }

        [WebMethod]
        public static string AddMembersToTeam(int teamId, List<MemberDTO> members)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                int successCount = 0;

                foreach (var member in members)
                {
                    // 1. Find User
                    string findUserQuery = "SELECT Username FROM Users WHERE Username = @Id OR Email = @Id";
                    SqlCommand findCmd = new SqlCommand(findUserQuery, conn);
                    findCmd.Parameters.AddWithValue("@Id", member.username);
                    object result = findCmd.ExecuteScalar();

                    if (result == null) continue; // Skip if user not found

                    string realUsername = result.ToString();

                    // 2. Check if already member
                    string checkMemberQuery = "SELECT COUNT(*) FROM TeamMembers WHERE TeamId = @TeamId AND Username = @Username";
                    SqlCommand checkCmd = new SqlCommand(checkMemberQuery, conn);
                    checkCmd.Parameters.AddWithValue("@TeamId", teamId);
                    checkCmd.Parameters.AddWithValue("@Username", realUsername);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0) continue; // Skip if already member

                    // 3. Add Member
                    string insertQuery = "INSERT INTO TeamMembers (TeamId, Username, Role) VALUES (@TeamId, @Username, @Role)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@TeamId", teamId);
                    insertCmd.Parameters.AddWithValue("@Username", realUsername);
                    insertCmd.Parameters.AddWithValue("@Role", member.role);
                    insertCmd.ExecuteNonQuery();
                    
                    successCount++;
                }

                if (successCount > 0)
                {
                    // 4. Update Team's UpdatedAt
                    string updateTeamQuery = "UPDATE Team SET UpdatedAt = GETDATE() WHERE TeamId = @TeamId";
                    SqlCommand updateCmd = new SqlCommand(updateTeamQuery, conn);
                    updateCmd.Parameters.AddWithValue("@TeamId", teamId);
                    updateCmd.ExecuteNonQuery();
                    
                    return "Success";
                }
                
                return "No new members were added.";
            }
        }

        [WebMethod]
        public static string AddMemberToTeam(int teamId, string username, string role)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 1. Find User
                string findUserQuery = "SELECT Username FROM Users WHERE Username = @Id OR Email = @Id";
                SqlCommand findCmd = new SqlCommand(findUserQuery, conn);
                findCmd.Parameters.AddWithValue("@Id", username);
                object result = findCmd.ExecuteScalar();

                if (result == null)
                {
                    return "User not found.";
                }

                string realUsername = result.ToString();

                // 2. Check if already member
                string checkMemberQuery = "SELECT COUNT(*) FROM TeamMembers WHERE TeamId = @TeamId AND Username = @Username";
                SqlCommand checkCmd = new SqlCommand(checkMemberQuery, conn);
                checkCmd.Parameters.AddWithValue("@TeamId", teamId);
                checkCmd.Parameters.AddWithValue("@Username", realUsername);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    return "User is already a member of this team.";
                }

                // 3. Add Member
                string insertQuery = "INSERT INTO TeamMembers (TeamId, Username, Role) VALUES (@TeamId, @Username, @Role)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@TeamId", teamId);
                insertCmd.Parameters.AddWithValue("@Username", realUsername);
                insertCmd.Parameters.AddWithValue("@Role", role);
                insertCmd.ExecuteNonQuery();

                // 4. Update Team's UpdatedAt
                string updateTeamQuery = "UPDATE Team SET UpdatedAt = GETDATE() WHERE TeamId = @TeamId";
                SqlCommand updateCmd = new SqlCommand(updateTeamQuery, conn);
                updateCmd.Parameters.AddWithValue("@TeamId", teamId);
                updateCmd.ExecuteNonQuery();

                return "Success";
            }
        }



        public class MemberDTO
        {
            public string username { get; set; }
            public string role { get; set; }
            public string avatar { get; set; }
        }

        protected void btnTeamUp_Click(object sender, EventArgs e)
        {
            string teamName = hfTeamName.Value;
            string membersJson = hfTeamMembers.Value;
            // Set default logo path initially
            string logoPath = "assets/images/teamwork.png";

            if (string.IsNullOrEmpty(teamName)) return;

            // Check if team name exists
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string checkTeamQuery = "SELECT COUNT(*) FROM Team WHERE TeamName = @Name";
                SqlCommand checkCmd = new SqlCommand(checkTeamQuery, conn);
                checkCmd.Parameters.AddWithValue("@Name", teamName);
                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    // Show error in modal
                    ClientScript.RegisterStartupScript(this.GetType(), "Pop", "showCreateTeamModal(); document.getElementById('lblCreateTeamError').innerText = 'Team name already exists. Please choose another name.'; document.getElementById('lblCreateTeamError').style.display = 'block';", true);
                    return;
                }
            }

            // Handle File Upload
            if (fuTeamLogo.HasFile)
            {
                try
                {
                    string filename = System.IO.Path.GetFileName(fuTeamLogo.FileName);
                    string extension = System.IO.Path.GetExtension(filename);
                    string uniqueName = Guid.NewGuid().ToString() + extension;
                    string folderPath = Server.MapPath("~/assets/images/teams/");
                    
                    if (!System.IO.Directory.Exists(folderPath))
                    {
                        System.IO.Directory.CreateDirectory(folderPath);
                    }

                    string savePath = System.IO.Path.Combine(folderPath, uniqueName);
                    fuTeamLogo.SaveAs(savePath);
                    logoPath = "assets/images/teams/" + uniqueName;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Upload error: " + ex.Message);
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Create Team
                    string insertTeamQuery = "INSERT INTO Team (TeamName, LogoPath, UpdatedAt, CreatorUsername) VALUES (@Name, @LogoPath, GETDATE(), @Creator); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdTeam = new SqlCommand(insertTeamQuery, conn, transaction);
                    cmdTeam.Parameters.AddWithValue("@Name", teamName);
                    cmdTeam.Parameters.AddWithValue("@LogoPath", logoPath); // Always store a path (default or uploaded)
                    cmdTeam.Parameters.AddWithValue("@Creator", Session["User"].ToString());
                    
                    object result = cmdTeam.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        throw new Exception("Failed to create team. ID not returned.");
                    }

                    int newTeamId = Convert.ToInt32(result);

                    // 2. Add Creator (Current User) as Admin
                    string currentUser = Session["User"].ToString();
                    UpdateUserTeam(conn, transaction, currentUser, newTeamId, "admin");

                    // 3. Add Members
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    MemberDTO[] members = serializer.Deserialize<MemberDTO[]>(membersJson);

                    if (members != null)
                    {
                        foreach (MemberDTO member in members)
                        {
                            UpdateUserTeam(conn, transaction, member.username, newTeamId, member.role);
                        }
                    }

                    transaction.Commit();
                    
                    // Redirect after successful commit
                    Response.Redirect(Request.RawUrl, false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    // Check if transaction is still active before rollback
                    if (transaction != null && transaction.Connection != null)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch { /* Ignore rollback errors */ }
                    }

                    lblError.Text = "Error creating team: " + ex.Message;
                    lblError.Visible = true;
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

                // Check if user is already in the team
                string checkMemberQuery = "SELECT COUNT(*) FROM TeamMembers WHERE TeamId = @TeamId AND Username = @Username";
                SqlCommand checkCmd = new SqlCommand(checkMemberQuery, conn, trans);
                checkCmd.Parameters.AddWithValue("@TeamId", teamId);
                checkCmd.Parameters.AddWithValue("@Username", username);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    // Insert new member
                    string insertQuery = @"
                        INSERT INTO TeamMembers (TeamId, Username, Role) 
                        VALUES (@TeamId, @Username, @Role)";
                    
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn, trans);
                    insertCmd.Parameters.AddWithValue("@TeamId", teamId);
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Role", role);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }
    }
}