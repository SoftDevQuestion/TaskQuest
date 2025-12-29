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
                        <div class="project-image">
                            <img src='<%# Eval("ImageUrl") %>' />
                        </div>

                        <div class="project-content">
                            <h3 class="project-title"><%# Eval("Title") %></h3>
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
            <span class="modal-close" onclick="closeProjectModal()">&times;</span>
            <h3>Create New Project</h3>
            
            <div class="form-group">
                <label>Project Name</label>
                <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Project Logo</label>
                <asp:FileUpload ID="fuProjectLogo" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label>Project Cover</label>
                <asp:FileUpload ID="fuProjectCover" runat="server" CssClass="form-control" />
            </div>

            <asp:Label ID="lblProjectError" runat="server" CssClass="error-message" EnableViewState="false"></asp:Label>

            <div class="btn-row">
                <button type="button" class="cancel-btn" onclick="closeProjectModal()">Cancel</button>
                <asp:Button ID="btnCreateProject" runat="server" Text="Create" CssClass="save-btn" OnClick="btnCreateProject_Click" />
            </div>
        </div>
    </div>

    <style>
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
            padding: 30px;
            border-radius: 12px;
            width: 400px;
            position: relative;
            box-shadow: 0 4px 20px rgba(0,0,0,0.15);
            animation: fadeIn 0.3s ease-out;
        }

        .modal-close {
            position: absolute;
            top: 15px;
            right: 20px;
            font-size: 24px;
            cursor: pointer;
            color: #aaa;
        }

        .modal-close:hover {
            color: #333;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: 500;
            color: #333;
        }

        .form-control {
            width: 100%;
            padding: 8px;
            border: 1px solid #ddd;
            border-radius: 6px;
            box-sizing: border-box;
        }

        .btn-row {
            display: flex;
            justify-content: flex-end;
            gap: 10px;
            margin-top: 20px;
        }

        .save-btn {
            background-color: #5577FF;
            color: white;
            border: none;
            padding: 8px 20px;
            border-radius: 6px;
            cursor: pointer;
        }

        .cancel-btn {
            background-color: #f0f0f0;
            color: #333;
            border: none;
            padding: 8px 20px;
            border-radius: 6px;
            cursor: pointer;
        }

        .error-message {
            color: #dc3545;
            font-size: 14px;
            margin-top: 10px;
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
    </script>
</asp:Content>
