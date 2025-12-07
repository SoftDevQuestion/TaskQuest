using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskQuest
{
    public partial class Profile : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptAvatars.DataSource = Enumerable.Range(1, 9)
                    .Select(i => $"assets/images/avatar{i}.png");
                rptAvatars.DataBind();

                imgAvatarPreview.ImageUrl = "assets/images/avatar1.png";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Text;
            string email = txtEmail.Text;

            string selectedAvatar = hdSelectedAvatar.Value;

            // TODO ذخیره در دیتابیس
        }
    }
}