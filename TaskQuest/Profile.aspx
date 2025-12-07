<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="TaskQuest.Profile" %>

<!DOCTYPE html>
<html>
<head>
    <title>Edit profile</title>
    <link rel="stylesheet" href="assets/css/profile.css" />
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
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="cancel-btn" />
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="save-btn"  />
        </div>

    </div>
        </div>
        

        <script>
            function chooseAvatar(url) {
                document.getElementById("<%=imgAvatarPreview.ClientID%>").src = url;
            document.getElementById("<%=hdSelectedAvatar.ClientID%>").value = url;
            }
        </script>

    <asp:HiddenField ID="hdSelectedAvatar" runat="server" />
    </form>
    

    

</body>
</html>