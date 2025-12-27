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
            //if (Session["User"] == null)
            //{
            //    Response.Redirect("Login.aspx");
            //    return;
            //}

            
        }

        
    }
}