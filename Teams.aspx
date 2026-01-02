<%@ Page Language="C#" MasterPageFile="~/SideBar.master" AutoEventWireup="true" CodeBehind="Teams.aspx.cs" Inherits="TaskQuest.Teams" %>



<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <title>Teams Page</title>
    <link rel="stylesheet" href="assets/css/teams.css">
</asp:Content>




<asp:Content ID="TeamsContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="teams-page">
        <div class="page-header">
            <h1 class="page-title">Team Management</h1>
            <button type="button" class="create-team-btn" onclick="showCreateTeamModal()">+ Create New Team</button>
        </div>
        
        <asp:Label ID="lblError" runat="server" CssClass="error-message" ForeColor="Red" EnableViewState="false"></asp:Label>

        <div class="teams-grid">
            <asp:Repeater ID="rptTeams" runat="server" OnItemCommand="rptTeams_ItemCommand">
                <ItemTemplate>
                    <section class="team-card">
                        <header class="team-card-header">
                            <div class="team-card-title">
                                <img src='<%# !string.IsNullOrEmpty(Eval("LogoPath") as string) ? Eval("LogoPath") : "assets/images/teamwork.png" %>' 
                                     alt="Team Logo" 
                                     class="team-logo" 
                                     style="width: 48px; height: 48px; border-radius: 12px; object-fit: cover; margin-right: 12px;" />
                                <div class="team-texts">
                                    <h2 class="team-name"><%# Eval("TeamName") %></h2>
                                    <p class="team-members-count"><%# Eval("MemberCount") %> members</p>
                                </div>
                            </div>
                            <div class="team-menu-container">
                                <button class="team-menu-btn" type="button" onclick="toggleMenu(this)">
                                    <span></span>
                                    <span></span>
                                    <span></span>
                                </button>
                                <div class="team-menu-dropdown">
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("TeamId") %>' CssClass="menu-item" CausesValidation="false">Edit</asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("TeamId") %>' CssClass="menu-item delete" OnClientClick="return confirm('Are you sure you want to delete this team?');" CausesValidation="false">Delete</asp:LinkButton>
                                </div>
                            </div>
                        </header>

                        <div class="team-members">
                            <asp:Repeater ID="rptMembers" runat="server" DataSource='<%# Eval("Members") %>'>
                                <ItemTemplate>
                                    <div class="member-row">
                                        <div class="member-info-with-avatar">
                                            <img src='<%# Eval("AvatarPath") %>' alt='<%# Eval("Username") %>' class="member-avatar" onerror="this.src='assets/images/default-avatar.svg'" />
                                            <div class="member-info">
                                                <p class="member-name"><%# Eval("Username") %></p>
                                                <p class="member-role"><%# Eval("Role") %></p>
                                            </div>
                                        </div>
                                        <span class='<%# "member-badge member-badge-" + Eval("Role").ToString().ToLower() %>'><%# Eval("Role") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <div class="team-card-footer">
                            <button class="add-member-btn" type="button" onclick="showAddMemberModal('<%# Eval("TeamId") %>')">
                                + Add member
                            </button>
                        </div>
                    </section>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <!-- Create Team Modal Structure -->
    <div id="createTeamModal" class="modal-overlay" style="display: none;">
        <div class="modal-content">
            <!-- Logo Circle Placeholder -->
            <div class="modal-logo-section">
                <div class="logo-circle" onclick="triggerFileUpload()">
                    <img id="imgLogoPreview" src="assets/images/default-team.png" class="logo-preview" style="display:none; width:100%; height:100%; border-radius:50%; object-fit:cover;" />
                    <span id="logoPlaceholderText">Upload Logo</span>
                </div>
                <asp:FileUpload ID="fuTeamLogo" runat="server" Style="display: none;" onchange="previewLogo(this)" ClientIDMode="Static" />
            </div>

            <!-- Team Name Input -->
            <div class="form-group">
                <label for="txtNewTeamName">Team Name</label>
                <input type="text" id="txtNewTeamName" class="form-control" placeholder="Enter team name..." />
            </div>

            <!-- Add Member Section -->
            <div class="members-section">
                <label>Members</label>
                <div id="membersList" class="members-list">
                    <!-- Dynamic rows will be added here -->
                </div>
                <button type="button" class="add-member-link" onclick="addNewMemberRow()">+ Add Member</button>
            </div>

            <!-- Footer Buttons -->
            <div class="modal-footer">
                <button type="button" class="btn-give-up" onclick="closeCreateTeamModal()">Give up</button>
                <asp:Button ID="btnTeamUp" runat="server" Text="Team up" CssClass="btn-team-up" OnClick="btnTeamUp_Click" OnClientClick="return prepareTeamData();" />
            </div>
        </div>
    </div>

    <!-- Edit Team Modal Structure -->
    <div id="editTeamModal" class="modal-overlay" style="display: none;" runat="server" clientidmode="Static">
        <div class="modal-content">
            <!-- Logo Circle Placeholder -->
            <div class="modal-logo-section">
                <div class="logo-circle" onclick="triggerEditFileUpload()">
                    <asp:Image ID="imgEditLogoPreview" runat="server" CssClass="logo-preview" Style="width:100%; height:100%; border-radius:50%; object-fit:cover;" ImageUrl="assets/images/default-team.png" ClientIDMode="Static" />
                    <span id="editLogoPlaceholderText" style="display:none;">Change Logo</span>
                </div>
                <asp:FileUpload ID="fuEditTeamLogo" runat="server" Style="display: none;" onchange="previewEditLogo(this)" ClientIDMode="Static" />
            </div>

            <!-- Team Name Input -->
            <div class="form-group">
                <label for="txtEditTeamName">Team Name</label>
                <asp:TextBox ID="txtEditTeamName" runat="server" CssClass="form-control" placeholder="Enter team name..." ClientIDMode="Static"></asp:TextBox>
            </div>

            <!-- Footer Buttons -->
            <div class="modal-footer">
                <button type="button" class="btn-give-up" onclick="closeEditTeamModal(); return false;">Cancel</button>
                <asp:Button ID="btnSaveEdit" runat="server" Text="Save Changes" CssClass="btn-team-up" OnClick="btnSaveEdit_Click" />
            </div>
        </div>
    </div>
    
    <!-- Add Member Modal Structure -->
     <div id="addMemberModal" class="modal-overlay" style="display: none;">
         <div class="modal-content" style="max-width: 320px;">
             <h3 style="margin: 0; font-size: 16px;">Add New Member</h3>
             <div class="form-group" style="margin-top: 12px;">
                 <label>Search User</label>
                 <div class="input-wrapper" style="position: relative;">
                     <input type="text" id="txtSearchAddMember" class="form-control" placeholder="Enter username or email..." onkeyup="searchMemberForExistingTeam(this)" />
                     <div id="addMemberSearchResults" class="search-results" style="display:none;"></div>
                 </div>
             </div>
             <div class="modal-footer" style="margin-top: 12px;">
                 <button type="button" class="btn-give-up" onclick="closeAddMemberModal()">Cancel</button>
             </div>
         </div>
     </div>
    
    <asp:HiddenField ID="hfEditTeamId" runat="server" />

    <asp:HiddenField ID="hfTeamMembers" runat="server" />
    <asp:HiddenField ID="hfTeamName" runat="server" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        function showCreateTeamModal() {
            document.getElementById('createTeamModal').style.display = 'flex';
            document.getElementById('txtNewTeamName').value = '';
            document.getElementById('membersList').innerHTML = '';
        }

        function closeCreateTeamModal() {
            document.getElementById('createTeamModal').style.display = 'none';
        }

        function showEditTeamModal() {
            document.getElementById('editTeamModal').style.display = 'flex';
        }

        function closeEditTeamModal() {
            document.getElementById('editTeamModal').style.display = 'none';
        }

        function triggerEditFileUpload() {
             var fileUpload = document.getElementById('fuEditTeamLogo');
             if (fileUpload) fileUpload.click();
        }

        function previewEditLogo(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function(e) {
                    document.getElementById('imgEditLogoPreview').src = e.target.result;
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        function addNewMemberRow() {
            const container = document.getElementById('membersList');
            const rowId = 'row_' + new Date().getTime();
            
            const rowHtml = `
                <div class="member-input-row" id="${rowId}">
                    <div class="input-wrapper">
                        <input type="text" class="member-search-input" placeholder="Username or Email" onkeyup="searchUsers(this, '${rowId}')" onkeydown="handleEnterKey(event, this)" />
                        <div class="search-results" style="display:none;"></div>
                    </div>
                    <button type="button" class="remove-row-btn" onclick="removeRow('${rowId}')">×</button>
                </div>
            `;
            
            // Append HTML
            container.insertAdjacentHTML('beforeend', rowHtml);
            
            // Focus new input
            const newRow = document.getElementById(rowId);
            newRow.querySelector('input').focus();
        }

        function removeRow(rowId) {
            document.getElementById(rowId).remove();
        }

        let searchTimeout;
        function searchUsers(input, rowId) {
            const term = input.value;
            const resultsDiv = input.parentElement.querySelector('.search-results');
            
            if (term.length < 2) {
                resultsDiv.style.display = 'none';
                return;
            }

            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                // Call WebMethod
                $.ajax({
                    type: "POST",
                    url: "Teams.aspx/SearchUsers",
                    data: JSON.stringify({ term: term }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        const users = response.d;
                        resultsDiv.innerHTML = '';
                        if (users.length > 0) {
                            users.forEach(user => {
                                const div = document.createElement('div');
                                div.className = 'search-result-item';
                                div.innerText = `${user.Username} (${user.Email})`;
                                div.onclick = () => selectUser(input, user.Username);
                                resultsDiv.appendChild(div);
                            });
                            resultsDiv.style.display = 'block';
                        } else {
                            resultsDiv.style.display = 'none';
                        }
                    },
                    error: function (err) {
                        console.error('Error searching users', err);
                    }
                });
            }, 300);
        }

        function selectUser(input, username) {
            input.value = username;
            input.parentElement.querySelector('.search-results').style.display = 'none';
            // Disable editing after selection? Or keep editable.
            // Prompt says: "ردیف ممبر کنونی سیو شود و ردیف جدیدی برای اضافه شدن ممبر جدید باز شود" (on Enter).
            // For click, we just select.
        }

        function handleEnterKey(e, input) {
            if (e.key === 'Enter') {
                e.preventDefault(); // Prevent form submit
                
                const resultsDiv = input.parentElement.querySelector('.search-results');
                // If results visible and has items, select first
                if (resultsDiv.style.display !== 'none' && resultsDiv.children.length > 0) {
                    const firstItem = resultsDiv.children[0];
                    // Trigger click on first item
                    firstItem.click();
                    
                    // Add new row
                    addNewMemberRow();
                }
            }
        }

        function prepareTeamData() {
            const teamName = document.getElementById('txtNewTeamName').value;
            if (!teamName) {
                alert('Please enter a team name.');
                return false;
            }

            // Collect members
            const memberInputs = document.querySelectorAll('.member-search-input');
            const members = [];
            memberInputs.forEach(input => {
                if (input.value.trim()) {
                    members.push(input.value.trim());
                }
            });

            document.getElementById('<%= hfTeamName.ClientID %>').value = teamName;
            document.getElementById('<%= hfTeamMembers.ClientID %>').value = JSON.stringify(members);
            
            return true;
        }

        // Close search results when clicking outside
        document.addEventListener('click', function(e) {
            if (!e.target.closest('.input-wrapper')) {
                document.querySelectorAll('.search-results').forEach(el => el.style.display = 'none');
            }
            
            // Close team menus when clicking outside
            if (!e.target.matches('.team-menu-btn') && !e.target.closest('.team-menu-btn')) {
                document.querySelectorAll('.team-menu-dropdown').forEach(el => {
                    el.classList.remove('show');
                });
            }
        });

        function toggleMenu(btn) {
            const dropdown = btn.nextElementSibling;
            // Close other open menus
            document.querySelectorAll('.team-menu-dropdown').forEach(el => {
                if (el !== dropdown) el.classList.remove('show');
            });
            dropdown.classList.toggle('show');
        }

        let currentAddMemberTeamId = null;

        function showAddMemberModal(teamId) {
            currentAddMemberTeamId = teamId;
            document.getElementById('addMemberModal').style.display = 'flex';
            document.getElementById('txtSearchAddMember').value = '';
            document.getElementById('addMemberSearchResults').style.display = 'none';
            document.getElementById('txtSearchAddMember').focus();
        }

        function closeAddMemberModal() {
            document.getElementById('addMemberModal').style.display = 'none';
            currentAddMemberTeamId = null;
        }

        let searchAddMemberTimeout;
        function searchMemberForExistingTeam(input) {
            const term = input.value;
            const resultsDiv = document.getElementById('addMemberSearchResults');
            
            if (term.length < 2) {
                resultsDiv.style.display = 'none';
                return;
            }

            clearTimeout(searchAddMemberTimeout);
            searchAddMemberTimeout = setTimeout(() => {
                $.ajax({
                    type: "POST",
                    url: "Teams.aspx/SearchUsers",
                    data: JSON.stringify({ term: term }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        const users = response.d;
                        resultsDiv.innerHTML = '';
                        if (users.length > 0) {
                            users.forEach(user => {
                                const div = document.createElement('div');
                                div.className = 'search-result-item';
                                div.innerText = `${user.Username} (${user.Email})`;
                                div.onclick = () => addMemberToTeam(user.Username);
                                resultsDiv.appendChild(div);
                            });
                            resultsDiv.style.display = 'block';
                        } else {
                            resultsDiv.style.display = 'none';
                        }
                    },
                    error: function (err) {
                        console.error('Error searching users', err);
                    }
                });
            }, 300);
        }

        function addMemberToTeam(username) {
            if (!currentAddMemberTeamId) return;
            
            // Just add without confirmation
            $.ajax({
                type: "POST",
                url: "Teams.aspx/AddMemberToTeam",
                data: JSON.stringify({ teamId: currentAddMemberTeamId, username: username }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === "Success") {
                        // No alert, just reload or close
                        closeAddMemberModal();
                        window.location.reload(); 
                    } else {
                        // Keep error alert if something goes wrong? 
                        // User said "don't give a message", but errors are different. 
                        // I will suppress success message as requested.
                        alert(response.d);
                    }
                },
                error: function (err) {
                    console.error('Error adding member', err);
                }
            });
        }

        function triggerFileUpload() {
            var fileUpload = document.getElementById('<%= fuTeamLogo.ClientID %>');
            if (fileUpload) {
                fileUpload.click();
            }
        }

        function previewLogo(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = document.getElementById('imgLogoPreview');
                    img.src = e.target.result;
                    img.style.display = 'block';
                    document.getElementById('logoPlaceholderText').style.display = 'none';
                }
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
</asp:Content>


