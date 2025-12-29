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
        }
        /* Success Popup Style */
        .success-popup {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background-color: #28a745; /* Green color */
            color: white;
            padding: 20px 40px;
            border-radius: 8px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.2);
            z-index: 2000;
            text-align: center;
            font-size: 18px;
            animation: fadeIn 0.3s ease-out;
        }
        /* Error Message Style */
        .error-message {
            color: #dc3545;
            display: block;
            margin-bottom: 15px;
            text-align: center;
            font-weight: bold;
        }
        @keyframes fadeIn {
            from { opacity: 0; transform: translate(-50%, -60%); }
            to { opacity: 1; transform: translate(-50%, -50%); }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div id="successPopup" class="success-popup">
            Changes saved successfully!
        </div>

        <div class="form-container">
            <div class="profile-modal">
                <h2>Edit Profile</h2>

                <asp:Image ID="imgAvatarPreview" runat="server" CssClass="avatar-large" ImageUrl="~/assets/images/avatar1.png" />

                <label class="upload-btn">
                    Upload
                    <asp:FileUpload ID="fuAvatar" runat="server" CssClass="hidden-file" />
                </label>

                <h4>Choose Avatar</h4>
                <div class="avatar-list">
                    <asp:Repeater ID="rptAvatars" runat="server">
                        <ItemTemplate>
                            <img class="avatar" src='<%# Container.DataItem %>' onclick="chooseAvatar('<%# Container.DataItem %>')" />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <label>Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" />

                <label>Username</label>
                <asp:TextBox ID="txtUser" runat="server" />

                <label>Password</label>
                <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />

                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" />

                <asp:Label ID="lblError" runat="server" CssClass="error-message" EnableViewState="false"></asp:Label>

                <div class="btn-row">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="cancel-btn" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="save-btn" OnClick="btnSave_Click" />
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
