<%@ Page Language="C#" MasterPageFile="~/SideBar.master"  AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="TaskQuest.Projects" %>



<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
<title>Projects Page</title>
    <link rel="stylesheet" href="assets/css/projects.css">
</asp:Content>




<asp:Content ID="ProjectContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="projects-header-row">
        <h2 style="padding:5px 20px;">Projects</h2>
        <button class="create-project-btn" onclick="openProjectModal(); return false;">+ Create New Project</button>
    </div>

    <div class="projects-container">
        <div class="projects-grid">
            <asp:Repeater ID="rptProjects" runat="server">
                <ItemTemplate>
                    <div class="project-card">
                        <div class="card-menu-container">
                            <span class="three-dots" onclick="toggleMenu(this)">&#8942;</span>
                            <div class="dropdown-menu">
                                <a href="#">Edit</a>
                                <a href="#">Delete</a>
                                <a href="#">Access</a>
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

    <!-- Create Project Modal -->
    <div id="projectModal" class="modal-overlay">
        <div class="modal-content">
            <!-- Header -->
            <div class="modal-header">
                <h3>Create Project</h3>
            </div>

            <!-- Visuals Area (Cover & Logo) -->
            <div class="visuals-container">
                <!-- Cover Image Area -->
                <div class="cover-upload" onclick="triggerFileUpload('fuProjectCover')">
                    <img id="imgCoverPreview" src="assets/images/default-project-cover.png" alt="Cover" />
                    <div class="upload-overlay"><span>Click to change cover</span></div>
                </div>

                <!-- Logo Image Area -->
                <div class="logo-upload" onclick="triggerFileUpload('fuProjectLogo')">
                    <img id="imgLogoPreview" src="assets/images/default-project-logo.png" alt="Logo" />
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
                <asp:Button ID="btnCreateProject" runat="server" Text="Create" CssClass="btn-create" OnClick="btnCreateProject_Click" />
            </div>
        </div>
    </div>

    <style>
        /* General Styles */
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

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

        /* Card Menu Styles */
        .project-card {
            position: relative;
        }

        .card-menu-container {
            position: absolute;
            top: 10px;
            left: 10px;
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
            left: 0;
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
        }

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
    </script>
</asp:Content>
