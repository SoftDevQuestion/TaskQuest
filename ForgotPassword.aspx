<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="TaskQuest.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>TaskQuest - Forgot Password</title>
    <link rel="stylesheet" href="assets/css/fonts.css">
    <link rel="stylesheet" href="assets/css/login.css">
    <style>
        body {
            font-family: 'LineIcons', 'Vazirmatn', Arial, sans-serif;
        }
    </style>
</head>
<body>
    <div class="login-container">
        <div class="login-card">
            <div class="login-header">
                <div class="material-logo">
                    <div class="logo-layers">
                        <div class="layer layer-1"></div>
                        <div class="layer layer-2"></div>
                        <div class="layer layer-3"></div>
                    </div>
                </div>
                <h2>Recovery</h2>
                <p>Reset your password</p>
            </div>
            
            <form class="login-form" id="forgotForm" runat="server" novalidate>
                
                <!-- Step 1: Identify User -->
                <asp:Panel ID="pnlStep1" runat="server">
                    <div class="form-group">
                        <div class="input-wrapper">
                            <asp:TextBox ID="txtIdentifier" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                            <label for="txtIdentifier">Email or Username</label>
                            <div class="input-line"></div>
                            <div class="ripple-container"></div>
                        </div>
                        <span class="error-message" id="identifierError" runat="server"></span>
                    </div>

                    <asp:Button ID="btnNext" runat="server" Text="NEXT" CssClass="login-btn material-btn" OnClick="btnNext_Click" />
                </asp:Panel>

                <!-- Step 2: Reset Password -->
                <asp:Panel ID="pnlStep2" runat="server" Visible="false">
                    <div class="form-group">
                        <div class="input-wrapper password-wrapper">
                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" required></asp:TextBox>
                            <label for="txtNewPassword">New Password</label>
                            <div class="input-line"></div>
                            <div class="ripple-container"></div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="input-wrapper password-wrapper">
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" required></asp:TextBox>
                            <label for="txtConfirmPassword">Confirm Password</label>
                            <div class="input-line"></div>
                            <div class="ripple-container"></div>
                        </div>
                        <span class="error-message" id="passwordError" runat="server"></span>
                    </div>

                    <asp:Button ID="btnReset" runat="server" Text="RESET PASSWORD" CssClass="login-btn material-btn" OnClick="btnReset_Click" />
                </asp:Panel>

                <div class="form-options" style="justify-content: center; margin-top: 20px;">
                    <a href="Login.aspx" class="forgot-password">Back to Login</a>
                </div>

            </form>
        </div>
    </div>

    <script src="assets/js/login.js"></script>
    <script>
        // Simple script to handle floating labels for ASP.NET controls which might mess up IDs
        document.addEventListener('DOMContentLoaded', function () {
            const inputs = document.querySelectorAll('input');
            inputs.forEach(input => {
                input.addEventListener('focus', () => {
                    input.parentElement.classList.add('focused');
                });
                input.addEventListener('blur', () => {
                    if (!input.value) {
                        input.parentElement.classList.remove('focused');
                    }
                });
                // Check initial state
                if (input.value) {
                    input.parentElement.classList.add('focused');
                }
            });
        });
    </script>
</body>
</html>