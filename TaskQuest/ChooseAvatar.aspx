<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseAvatar.aspx.cs" Inherits="TaskQuest.ChooseAvatar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>TaskQuest - انتخاب آواتار</title>
    <link rel="stylesheet" href="assets/css/choose-avatar.css">
</head>
<body>
    <form id="form1" runat="server">
        <div style="display:none;">
            <asp:HiddenField ID="encodingTest" runat="server" Value="تست انکودینگ فارسی" />
        </div>
        <div class="choose-avatar-container">
            <div class="avatar-card">
                <asp:Label ID="ErrorMessage" runat="server" CssClass="error-message" Visible="false"></asp:Label>
                <div class="avatar-header">
                    <div class="material-logo">
                        <div class="logo-layers">
                            <div class="layer layer-1"></div>
                            <div class="layer layer-2"></div>
                            <div class="layer layer-3"></div>
                        </div>
                    </div>
                    <h2><asp:Label ID="lblHeader" runat="server" Text="تکمیل پروفایل"></asp:Label></h2>
                    <p><asp:Label ID="lblSubHeader" runat="server" Text="لطفاً عکس پروفایل خود را انتخاب کنید"></asp:Label></p>
                </div>

                <div class="avatar-selection">
                    <h3><asp:Label ID="lblDefaultAvatars" runat="server" Text="عکس‌های پیش‌فرض"></asp:Label></h3>
                    <div class="default-avatars">
                        <div class="avatar-option" data-avatar="avatar1.svg">
                            <img src="assets/images/avatars/avatar1.svg" alt="Avatar 1" />
                            <div class="avatar-overlay">
                                <span class="check-icon">✓</span>
                            </div>
                        </div>
                        <div class="avatar-option" data-avatar="avatar2.svg">
                            <img src="assets/images/avatars/avatar2.svg" alt="Avatar 2" />
                            <div class="avatar-overlay">
                                <span class="check-icon">✓</span>
                            </div>
                        </div>
                        <div class="avatar-option" data-avatar="avatar3.svg">
                            <img src="assets/images/avatars/avatar3.svg" alt="Avatar 3" />
                            <div class="avatar-overlay">
                                <span class="check-icon">✓</span>
                            </div>
                        </div>
                        <div class="avatar-option" data-avatar="avatar4.svg">
                            <img src="assets/images/avatars/avatar4.svg" alt="Avatar 4" />
                            <div class="avatar-overlay">
                                <span class="check-icon">✓</span>
                            </div>
                        </div>
                        <div class="avatar-option" data-avatar="avatar5.svg">
                            <img src="assets/images/avatars/avatar5.svg" alt="Avatar 5" />
                            <div class="avatar-overlay">
                                <span class="check-icon">✓</span>
                            </div>
                        </div>
                        <div class="avatar-option" data-avatar="avatar6.svg">
                            <img src="assets/images/avatars/avatar6.svg" alt="Avatar 6" />
                            <div class="avatar-overlay">
                                <span class="check-icon">✓</span>
                            </div>
                        </div>
                    </div>

                    <div class="upload-section">
                        <h3><asp:Label ID="lblUploadTitle" runat="server" Text="یا عکس خود را آپلود کنید"></asp:Label></h3>
                        <div class="upload-area" id="uploadArea">
                            <div class="upload-icon">📁</div>
                            <p>برای آپلود عکس اینجا کلیک کنید یا عکس را بکشید و اینجا رها کنید</p>
                            <input type="file" id="avatarUpload" accept="image/*" style="display: none;" />
                            <asp:FileUpload ID="fileUpload" runat="server" CssClass="file-upload" />
                        </div>
                        <div class="upload-preview" id="uploadPreview" style="display: none;">
                            <img id="previewImage" src="" alt="Preview" />
                            <button type="button" class="remove-preview" onclick="removeUpload()">×</button>
                        </div>
                    </div>
                </div>

                <div class="selected-avatar-info" id="selectedInfo" style="display: none;">
                    <p>آواتار انتخاب شده: <span id="selectedAvatarName"></span></p>
                </div>

                <div class="avatar-actions">
                    <asp:HiddenField ID="selectedAvatarPath" runat="server" Value="" />
                    <asp:Button ID="skipButton" runat="server" Text="فعلاً از این مرحله بگذر" CssClass="btn btn-secondary" OnClick="SkipButton_Click" />
                    <asp:Button ID="completeButton" runat="server" Text="انجام شد" CssClass="btn btn-primary" OnClick="CompleteButton_Click" />
                    <asp:HiddenField ID="hiddenSkipText" runat="server" Value="فعلاً از این مرحله بگذر" />
                    <asp:HiddenField ID="hiddenCompleteText" runat="server" Value="انجام شد" />
                </div>
            </div>
        </div>
    </form>

    <script src="assets/js/choose-avatar.js"></script>
</body>
</html>