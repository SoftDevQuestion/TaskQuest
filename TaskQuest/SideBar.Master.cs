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

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();
            // اینجا می‌تونی سرچ رو هندل کنی، مثلاً ریدایرکت یا فیلتر دیتا
            Response.Redirect("SearchResults.aspx?q=" + Server.UrlEncode(query));
        }
    }
}