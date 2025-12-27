using System;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Collections.Generic;
namespace TaskQuest
{
    public partial class Projects : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            //if (Session["User"] == null)
            //{
            //    Response.Redirect("Login.aspx");
            //    return;
            //}

            if (!IsPostBack)
            {
                LoadProjects();
            }
        }
        private void LoadProjects()
        {
            var projects = new List<ProjectVM>
    {
        new ProjectVM
        {
            Title = "Physics",
            Description = "Small and concise headline for physics project",
            ImageUrl = "assets/images/project1.jpeg"
        },
        new ProjectVM
        {
            Title = "UI/UX Design System",
            Description = "Longer description that should not break the layout",
            ImageUrl = "assets/images/project2.jpeg"
        }
    };

            rptProjects.DataSource = projects;
            rptProjects.DataBind();
        }

        public class ProjectVM
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
        }


    }
}