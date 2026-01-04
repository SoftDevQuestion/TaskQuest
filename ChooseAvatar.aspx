<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseAvatar.aspx.cs" Inherits="TaskQuest.ChooseAvatar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>TaskQuest - Choose Avatar</title>
    <link rel="stylesheet" href="assets/css/fonts.css" />
    <link rel="stylesheet" href="assets/css/profile.css" />
    <style>
        body {
            background-color: #f4f4f4; /* Match profile.css or desired background */
            margin: 0;
            font-family: 'PlusJakartaSans', Arial, sans-serif;
        }
        .profile-modal {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div class="form-container">
            <div class="profile-modal">
                <asp:Label ID="ErrorMessage" runat="server" CssClass="error-message" Visible="false" ForeColor="Red"></asp:Label>
                
                <h2><asp:Label ID="lblHeader" runat="server" Text="Complete Your Profile"></asp:Label></h2>
                <p><asp:Label ID="lblSubHeader" runat="server" Text="Please select a profile picture"></asp:Label></p>

                <!-- Preview Image -->
                <img id="imgAvatarPreview" src="assets/images/avatar1.png" class="avatar-large" style="width: 100px; height: 100px; border-radius: 50%; margin: 10px auto; display: block;" />

                <!-- Upload Section -->
                <div class="upload-section" style="margin: 15px 0;">
                    <label class="upload-btn">
                        <asp:Label ID="lblUploadTitle" runat="server" Text="Or upload your own photo"></asp:Label>
                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="hidden-file" onchange="previewUpload(this)" />
                    </label>
                </div>

                <h4><asp:Label ID="lblDefaultAvatars" runat="server" Text="Preset Avatars"></asp:Label></h4>
                
                <div class="avatar-list" style="justify-content: center;">
                    <img class="avatar" src="assets/images/avatar1.png" onclick="chooseAvatar('assets/images/avatar1.png')" />
                    <img class="avatar" src="assets/images/avatar2.png" onclick="chooseAvatar('assets/images/avatar2.png')" />
                    <img class="avatar" src="assets/images/avatar3.png" onclick="chooseAvatar('assets/images/avatar3.png')" />
                    <img class="avatar" src="assets/images/avatar4.png" onclick="chooseAvatar('assets/images/avatar4.png')" />
                    <img class="avatar" src="assets/images/avatar5.png" onclick="chooseAvatar('assets/images/avatar5.png')" />
                    <img class="avatar" src="assets/images/avatar6.png" onclick="chooseAvatar('assets/images/avatar6.png')" />
                    <img class="avatar" src="assets/images/avatar7.png" onclick="chooseAvatar('assets/images/avatar7.png')" />
                    <img class="avatar" src="assets/images/avatar8.png" onclick="chooseAvatar('assets/images/avatar8.png')" />

                </div>

                <div class="btn-row" style="justify-content: center; margin-top: 20px;">
                    <asp:Button ID="skipButton" runat="server" Text="Skip for Now" CssClass="cancel-btn" OnClick="SkipButton_Click" />
                    <asp:Button ID="completeButton" runat="server" Text="Confirm" CssClass="save-btn" OnClick="CompleteButton_Click" />
                </div>

                <asp:HiddenField ID="selectedAvatarPath" runat="server" Value="" />
                <asp:HiddenField ID="hiddenSkipText" runat="server" Value="Skip for now" />
                <asp:HiddenField ID="hiddenCompleteText" runat="server" Value="Confirm" />
            </div>
        </div>
    </form>

    <script>
        function chooseAvatar(url) {
            document.getElementById("imgAvatarPreview").src = url;
            document.getElementById("<%=selectedAvatarPath.ClientID%>").value = url;
            
            // Highlight selected
            var avatars = document.querySelectorAll('.avatar');
            avatars.forEach(av => av.style.borderColor = 'transparent');
            event.target.style.borderColor = '#007bff';
        }

        function previewUpload(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById("imgAvatarPreview").src = e.target.result;
                    // We can't easily set the file path to hidden field for upload, 
                    // the FileUpload control handles the file content on postback.
                    // But we should clear the selected avatar path if user uploads custom.
                    document.getElementById("<%=selectedAvatarPath.ClientID%>").value = ""; 
                }
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
</body>
</html>
