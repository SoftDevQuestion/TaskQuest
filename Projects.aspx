<%@ Page Language="C#" MasterPageFile="~/SideBar.master"  AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="TaskQuest.Projects" ValidateRequest="false" %>



<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
<title>Projects Page</title>
    <link rel="stylesheet" href="assets/css/projects.css">
</asp:Content>




<asp:Content ID="ProjectContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upProjects" runat="server">
        <ContentTemplate>
    <div class="projects-header-row">
        <h2 style="padding:5px 20px;">Projects</h2>
        <button class="create-project-btn" onclick="openProjectModal(); return false;">+ Create New Project</button>
    </div>

    <div class="projects-container">
        <asp:Label ID="lblNoProjects" runat="server" Text="No projects found. Create one!" Visible="false" CssClass="no-projects-msg"></asp:Label>
        
        <!-- Hidden Fields for State Management -->
        <asp:HiddenField ID="hfEditProjectId" runat="server" />
        <asp:HiddenField ID="hfDeleteProjectId" runat="server" />
        <asp:HiddenField ID="hfAccessProjectId" runat="server" />

        <div class="projects-grid">
            <asp:Repeater ID="rptProjects" runat="server" OnItemCommand="rptProjects_ItemCommand">
                <ItemTemplate>
                    <div class="project-card">
                        <div class="card-menu-container">
                            <span class="three-dots" onclick="toggleMenu(this)">&#8942;</span>
                            <div class="dropdown-menu">
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("ProjectID") %>' Visible='<%# IsProjectAdmin(Eval("CanEdit")) %>'>Edit</asp:LinkButton>
                                <a href="#" onclick="openDeleteModal('<%# Eval("ProjectID") %>'); return false;" style='<%# IsProjectAdmin(Eval("CanEdit")) ? "" : "display:none" %>'>Delete</a>
                                <asp:LinkButton ID="btnAccess" runat="server" CommandName="Access" CommandArgument='<%# Eval("ProjectID") %>' Visible='<%# IsProjectAdmin(Eval("CanEdit")) %>'>Access</asp:LinkButton>
                            </div>
                        </div>

                        <div class="project-image">
                            <img src='<%# Eval("ProjectCover") %>' />
                        </div>

                        <div class="project-content">
                            <h3 class="project-title"><%# Eval("ProjectName") %></h3>
                            <p class="project-description"><%# Eval("Description") %></p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <!-- Create/Edit Project Modal -->
    <div id="projectModal" class="modal-overlay">
        <div class="modal-content">
            <!-- Header -->
            <div class="modal-header">
                <h3><asp:Label ID="lblModalTitle" runat="server" Text="Create Project"></asp:Label></h3>
            </div>

            <!-- Visuals Area (Cover & Logo) -->
            <div class="visuals-container">
                <!-- Cover Image Area -->
                <div class="cover-upload" onclick="triggerFileUpload('fuProjectCover')">
                    <asp:Image ID="imgCoverPreview" runat="server" ImageUrl="assets/images/default_cover.jpg" ClientIDMode="Static" AlternateText="Cover" />
                    <div class="upload-overlay"><span>Click to change cover</span></div>
                </div>

                <!-- Logo Image Area -->
                <div class="logo-upload" onclick="triggerFileUpload('fuProjectLogo')">
                    <asp:Image ID="imgLogoPreview" runat="server" ImageUrl="assets/images/projectTestAvatar.jpg" ClientIDMode="Static" AlternateText="Logo" />
                    <div class="logo-overlay"><span>Logo</span></div>
                </div>
            </div>

            <!-- Hidden File Uploads -->
            <div style="display:none;">
                <asp:FileUpload ID="fuProjectLogo" runat="server" ClientIDMode="Static" onchange="previewImage(this, 'imgLogoPreview')" />
                <asp:FileUpload ID="fuProjectCover" runat="server" ClientIDMode="Static" onchange="previewImage(this, 'imgCoverPreview')" />
            </div>

            <!-- Inputs -->
            <div class="form-container">
                <div class="form-group">
                    <label>Project Name</label>
                    <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" placeholder="Enter project name"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>Project Description</label>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Enter project description"></asp:TextBox>
                </div>

                <asp:Label ID="lblProjectError" runat="server" CssClass="error-message" EnableViewState="false"></asp:Label>
            </div>

            <!-- Footer Buttons -->
            <div class="modal-footer">
                <button type="button" class="btn-give-up" onclick="closeProjectModal()">Give up</button>
                
                <!-- Visible Trigger Button -->
                <button type="button" id="btnVisibleCreate" runat="server" class="btn-create" onclick="handleSaveProject()">Create</button>

                <!-- Hidden Logic Buttons -->
                <asp:Button ID="btnSaveFull" runat="server" Text="SaveFull" OnClick="btnCreateProject_Click" Style="display:none;" />
                <asp:Button ID="btnSaveAsync" runat="server" Text="SaveAsync" OnClick="btnCreateProject_Click" Style="display:none;" />
            </div>
        </div>
    </div>

    <!-- Access Modal -->
    <div id="accessModal" class="modal-overlay" style="display:none;">
        <div class="modal-content" style="width: 500px;">
            <div class="modal-header">
                <h3>Team Access</h3>
                <span class="close-modal" onclick="closeAccessModal()">×</span>
            </div>
            <div class="form-container">
                <div class="form-group" style="position: relative;">
                    <label>Teams</label>
                    <div class="search-wrapper">
                        <asp:TextBox ID="txtSearchTeam" runat="server" CssClass="form-control search-input" placeholder="Search Teams" AutoPostBack="true" OnTextChanged="txtSearchTeam_TextChanged" autocomplete="off"></asp:TextBox>
                        <i class="search-icon">🔍</i>
                    </div>

                    <!-- Search Results Dropdown -->
                    <asp:Panel ID="pnlSearchResults" runat="server" CssClass="search-results-dropdown" Visible="false">
                        <asp:Repeater ID="rptSearchResults" runat="server" OnItemCommand="rptSearchResults_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAddTeam" runat="server" CommandName="Add" CommandArgument='<%# Eval("TeamId") %>' CssClass="search-result-item">
                                    <img src='<%# !string.IsNullOrEmpty(Eval("LogoPath") as string) ? Eval("LogoPath") : "assets/images/teamwork.png" %>' class="team-mini-logo" />
                                    <div class="team-info">
                                        <span class="team-name"><%# Eval("TeamName") %></span>
                                        <span class="team-desc"><%# Eval("Description") %></span>
                                    </div>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                </div>

                <!-- Selected Teams List -->
                <div class="selected-teams-list">
                    <asp:Repeater ID="rptAssignedTeams" runat="server" OnItemCommand="rptAssignedTeams_ItemCommand">
                        <ItemTemplate>
                            <div class="assigned-team-item">
                                <div class="team-visual">
                                    <img src='<%# !string.IsNullOrEmpty(Eval("LogoPath") as string) ? Eval("LogoPath") : "assets/images/teamwork.png" %>' class="team-avatar" />
                                    <div class="team-details">
                                        <span class="team-title"><%# Eval("TeamName") %></span>
                                        <span class="team-subtitle"><%# Eval("Description") %></span>
                                    </div>
                                </div>
                                <asp:LinkButton ID="btnRemoveTeam" runat="server" CommandName="Remove" CommandArgument='<%# Eval("TeamId") %>' CssClass="btn-remove-team">×</asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                
                <asp:Label ID="lblAccessError" runat="server" CssClass="error-message"></asp:Label>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn-cancel" onclick="closeAccessModal()">Cancel</button>
                <asp:Button ID="btnSaveAccess" runat="server" Text="invite" OnClick="btnSaveAccess_Click" CssClass="btn-create" style="background-color: #3b82f6;" />
            </div>
        </div>
    </div>

    <script>
        function openAccessModal() {
            document.getElementById('accessModal').style.display = 'flex';
        }
        function closeAccessModal() {
            document.getElementById('accessModal').style.display = 'none';
        }
    </script>

    <!-- Delete Confirmation Modal -->
    <div id="deleteModal" class="modal-overlay">
        <div class="modal-content" style="width: 400px; height: auto;">
            <div class="modal-header">
                <h3 style="color: #dc3545;">Delete Project</h3>
            </div>
            <div style="padding: 20px; text-align: center;">
                <p>Are you sure about deleting this project? This action can not be undone!</p>
            </div>
            <div class="modal-footer" style="justify-content: center;">
                <button type="button" class="btn-give-up" onclick="closeDeleteModal()">No</button>
                <asp:Button ID="btnConfirmDelete" runat="server" Text="Yes" CssClass="btn-create" Style="background-color: #dc3545;" OnClick="btnConfirmDelete_Click" />
            </div>
        </div>
    </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveFull" />
        </Triggers>
    </asp:UpdatePanel>

    <style>
        /* General Styles */
        
        .projects-header-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 0 20px;
        }

        .create-project-btn {
            background-color: #5577FF;
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 8px;
            cursor: pointer;
            font-size: 14px;
            font-weight: bold;
            transition: background 0.3s;
        }

        .create-project-btn:hover {
            background-color: #3355DD;
        }

        .no-projects-msg {
            display: block;
            text-align: center;
            color: #666;
            font-size: 18px;
            margin-top: 50px;
        }

        /* Card Menu Styles */
        .project-card {
            position: relative;
        }

        .card-menu-container {
            position: absolute;
            top: 10px;
            right: 10px;
            z-index: 10;
        }

        .three-dots {
            cursor: pointer;
            font-size: 24px;
            color: white;
            text-shadow: 0 0 3px rgba(0,0,0,0.5);
            font-weight: bold;
            user-select: none;
        }

        .dropdown-menu {
            display: none;
            position: absolute;
            top: 30px;
            right: 0;
            left: auto;
            background-color: white;
            box-shadow: 0 2px 5px rgba(0,0,0,0.2);
            border-radius: 4px;
            min-width: 100px;
            z-index: 20;
        }

        .dropdown-menu a {
            display: block;
            padding: 8px 12px;
            text-decoration: none;
            color: #333;
            font-size: 14px;
        }

        .dropdown-menu a:hover {
            background-color: #f5f5f5;
        }

        /* Modal Styles */
        .modal-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.5);
            z-index: 1000;
            justify-content: center;
            align-items: center;
        }

        .modal-content {
            background-color: white;
            width: 500px;
            border-radius: 20px;
            overflow: hidden;
            box-shadow: 0 10px 30px rgba(0,0,0,0.2);
            animation: fadeIn 0.3s ease-out;
            padding-bottom: 20px;
            position: relative;
        }

        .modal-header {
            text-align: center;
            padding: 20px 0 10px;
        }

        .modal-header h3 {
            margin: 0;
            font-size: 18px;
            font-weight: 600;
            color: #000;
        }

        /* Visuals Area */
        .visuals-container {
            position: relative;
            width: 100%;
            height: 180px; /* Adjust based on cover aspect ratio */
            margin-bottom: 40px; /* Space for the logo to hang out */
            padding: 0 20px; /* Padding for the cover inside modal */
            box-sizing: border-box;
        }

        .cover-upload {
            width: 100%;
            height: 100%;
            border-radius: 15px;
            overflow: hidden;
            position: relative;
            cursor: pointer;
            background-color: #f0f0f0;
        }

        .cover-upload img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

        .logo-upload {
            width: 80px;
            height: 80px;
            border-radius: 50%;
            border: 4px solid white;
            position: absolute;
            bottom: -25px; /* Half overlapping */
            left: 40px;
            cursor: pointer;
            background-color: #e0e0e0;
            overflow: hidden;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
            z-index: 5;
        }

        .logo-upload img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }
        
        /* Overlays for hover effect */
        .upload-overlay, .logo-overlay {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.3);
            display: flex;
            justify-content: center;
            align-items: center;
            opacity: 0;
            transition: opacity 0.2s;
            color: white;
            font-size: 12px;
            font-weight: bold;
        }

        .cover-upload:hover .upload-overlay,
        .logo-upload:hover .logo-overlay {
            opacity: 1;
        }

        /* Form Fields */
        .form-container {
            padding: 0 30px;
            margin-top: 10px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            margin-bottom: 8px;
            font-weight: 600;
            font-size: 14px;
            color: #000;
        }

        .form-control {
            width: 100%;
            padding: 12px 15px;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            background-color: #f9f9f9;
            font-size: 14px;
            box-sizing: border-box;
            outline: none;
            transition: border-color 0.2s;
        }

        .form-control:focus {
            border-color: #5577FF;
            background-color: #fff;
        }

        .form-control::placeholder {
            color: #aaa;
        }

        /* Buttons */
        .modal-footer {
            padding: 10px 30px;
            display: flex;
            justify-content: flex-end;
            gap: 15px;
        }

        .btn-give-up {
            background-color: #f0f2f5;
            color: #333;
            border: none;
            padding: 10px 20px;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            font-size: 14px;
        }

        .btn-give-up:hover {
            background-color: #e4e6e9;
        }

        .btn-create {
            background-color: #050510; /* Dark color as in image */
            color: white;
            border: none;
            padding: 10px 25px;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            font-size: 14px;
        }

        .btn-create:hover {
            background-color: #222;
        }

        .error-message {
            color: #dc3545;
            font-size: 13px;
            margin-top: -10px;
            margin-bottom: 10px;
            display: block;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(-20px); }
            to { opacity: 1; transform: translateY(0); }
        }
    </style>

    <script>
        function openProjectModal() {
            document.getElementById('projectModal').style.display = 'flex';
        }

        function closeProjectModal() {
            document.getElementById('projectModal').style.display = 'none';
            // Clear the form if needed or reset via postback logic
        }

        function openDeleteModal(projectId) {
            document.getElementById('<%= hfDeleteProjectId.ClientID %>').value = projectId;
            document.getElementById('deleteModal').style.display = 'flex';
        }

        function closeDeleteModal() {
            document.getElementById('deleteModal').style.display = 'none';
        }
        
        // Check for action=create in URL
        document.addEventListener('DOMContentLoaded', function() {
            const urlParams = new URLSearchParams(window.location.search);
            if (urlParams.get('action') === 'create') {
                openProjectModal();
                // Optional: Clean up URL
                // window.history.replaceState({}, document.title, window.location.pathname);
            }
        });

        function triggerFileUpload(id) {
            document.getElementById(id).click();
        }

        function previewImage(input, previewId) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById(previewId).src = e.target.result;
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        function toggleMenu(element) {
            // Close all other menus first
            var dropdowns = document.getElementsByClassName("dropdown-menu");
            for (var i = 0; i < dropdowns.length; i++) {
                if (dropdowns[i] !== element.nextElementSibling) {
                    dropdowns[i].style.display = "none";
                }
            }

            // Toggle current menu
            var dropdown = element.nextElementSibling;
            if (dropdown.style.display === "block") {
                dropdown.style.display = "none";
            } else {
                dropdown.style.display = "block";
            }
            
            // Prevent event bubbling so document click doesn't close it immediately
            event.stopPropagation();
        }

        // Close menu when clicking outside
        document.addEventListener('click', function(event) {
            var dropdowns = document.getElementsByClassName("dropdown-menu");
            for (var i = 0; i < dropdowns.length; i++) {
                dropdowns[i].style.display = "none";
            }
        });

        function handleSaveProject() {
            var logo = document.getElementById('fuProjectLogo');
            var cover = document.getElementById('fuProjectCover');
            var hasFiles = (logo && logo.files && logo.files.length > 0) || 
                           (cover && cover.files && cover.files.length > 0);
            
            if (hasFiles) {
                document.getElementById('<%= btnSaveFull.ClientID %>').click();
            } else {
                document.getElementById('<%= btnSaveAsync.ClientID %>').click();
            }
        }
    </script>
</asp:Content>
