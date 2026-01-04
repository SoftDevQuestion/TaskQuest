using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskQuest
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] != null)
                {
                    // User is logged in and session is active
                    lnkLoginIcon.HRef = "Dashboard.aspx";
                    lnkGetStarted.HRef = "Dashboard.aspx";
                }
                else
                {
                    // Session expired or not logged in
                    lnkLoginIcon.HRef = "Login.aspx";
                    lnkGetStarted.HRef = "Login.aspx";
                }
            }
        }
    }
}