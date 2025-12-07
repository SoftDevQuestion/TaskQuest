using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;

namespace TaskQuest
{
    public partial class Tasks : BasePage
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
            string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;

            // Set all Persian text with proper encoding
            SetControlText(lblLogo, "TaskQuest");
            SetControlText(lblNavDashboard, "داشبورد");
            SetControlText(lblNavTasks, "وظایف");
            SetControlText(lblNavProjects, "پروژه‌ها");
            SetControlText(lblNavTeams, "تیم‌ها");
            SetControlText(lblUsername, "کاربر");
            SetControlText(lblLogout, "خروج");
            SetControlText(lblPageTitle, "مدیریت وظایف");
            SetControlText(lblAddTask, "افزودن وظیفه جدید");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT FullName FROM Users WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string fullName = reader["FullName"].ToString();
                    SetControlText(lblUsername, fullName);
                }
                reader.Close();
            }
        }
    }
}