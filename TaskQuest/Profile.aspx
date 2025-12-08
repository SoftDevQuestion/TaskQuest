<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="TaskQuest.Profile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit profile</title>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="assets/css/fonts.css" />
    <link rel="stylesheet" href="assets/css/profile.css" />
    <style>
        body {
            background-color: #f4f4f4; /* Light grey background */
            margin: 0;
            font-family: 'Vazirmatn', Arial, sans-serif;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <div class="profile-modal">
                <h2>My Profile</h2>

                <asp:Image ID="imgAvatarPreview" runat="server" CssClass="avatar-large" ImageUrl="~/assets/images/avatar1.png" />

                <label class="upload-btn">
                    Upload
                    <asp:FileUpload ID="fuAvatar" runat="server" CssClass="hidden-file" />
                </label>

                <h4>Choose avatar</h4>
                <div class="avatar-list">
                    <asp:Repeater ID="rptAvatars" runat="server">
                        <ItemTemplate>
                            <img class="avatar" src='<%# Container.DataItem %>' onclick="chooseAvatar('<%# Container.DataItem %>')" />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <label>Username</label>
                <asp:TextBox ID="txtUser" runat="server" />

                <label>Password</label>
                <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />

                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" />

                <div class="btn-row">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="cancel-btn" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="save-btn" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdSelectedAvatar" runat="server" />

        <script>
            function chooseAvatar(url) {
                var preview = document.getElementById("<%=imgAvatarPreview.ClientID%>");
                if (preview) preview.src = url;
                
                var hidden = document.getElementById("<%=hdSelectedAvatar.ClientID%>");
                if (hidden) hidden.value = url;
            }
        </script>
    </form>
</body>
</html>
