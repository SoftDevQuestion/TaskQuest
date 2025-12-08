using System;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TaskQuest
{
    public partial class Projects : System.Web.UI.Page
    {
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

       
    }
}