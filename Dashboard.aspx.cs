using System;
using System.Web;
using System.Web.UI;

namespace TaskQuest
{
    public partial class Dashboard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if user is logged in
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LoadUserInfo();
            }
        }

        private void LoadUserInfo()
        {
            // Set welcome message with proper encoding
            SetControlText(lblWelcome, "کاربر گرامی خوش آمدید");
        }
    }
}