using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskQuest
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Clear Session
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            // 2. Clear Auth Cookie
            if (Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName] != null)
            {
                HttpCookie myCookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName);
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            // 3. Clear ASP.NET Session ID Cookie
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                HttpCookie sessionCookie = new HttpCookie("ASP.NET_SessionId");
                sessionCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(sessionCookie);
            }

            // 4. Redirect to Login
            Response.Redirect("Login.aspx");
        }
    }
}