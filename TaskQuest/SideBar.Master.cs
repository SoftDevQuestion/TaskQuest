using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskQuest
{
    public partial class SideBar : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateProfileInfo();
            }
            SetActiveMenu();
        }

        private void SetActiveMenu()
        {
            string activePage = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            // Reset all classes
            liDashboard.Attributes["class"] = "";
            liTasks.Attributes["class"] = "";
            liTeams.Attributes["class"] = "";
            liProjects.Attributes["class"] = "";

            switch (activePage)
            {
                case "dashboard.aspx":
                    liDashboard.Attributes["class"] = "active";
                    break;
                case "tasks.aspx":
                    liTasks.Attributes["class"] = "active";
                    break;
                case "teams.aspx":
                    liTeams.Attributes["class"] = "active";
                    break;
                case "projects.aspx":
                    liProjects.Attributes["class"] = "active";
                    break;
            }
        }

        public void UpdateProfileInfo()
        {
            if (Session["User"] != null)
            {
                lblProfileName.Text = Session["User"].ToString();
                
                if (Session["UserAvatar"] != null)
                {
                    imgProfileAvatar.ImageUrl = Session["UserAvatar"].ToString();
                }
                else
                {
                    // Fallback to check DB or use default
                    // For now, let's assume if session is empty, we might need to load it (or just use default)
                    // In a real scenario, you might want to fetch from DB if Session["UserAvatar"] is null but User is logged in
                    // But for performance, relying on Session set during Login/ProfileUpdate is better.
                    imgProfileAvatar.ImageUrl = "assets/images/default-avatar.png";
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();
            // اینجا می‌تونی سرچ رو هندل کنی، مثلاً ریدایرکت یا فیلتر دیتا
            Response.Redirect("SearchResults.aspx?q=" + Server.UrlEncode(query), false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}