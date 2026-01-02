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

        <div class="teams-grid">
            <!-- Team card 1 -->
            <section class="team-card">
                <header class="team-card-header">
                    <div class="team-card-title">
                        <span class="team-emoji">💻</span>
                        <div class="team-texts">
                            <h2 class="team-name">Development Team</h2>
                            <p class="team-members-count">3 members</p>
                        </div>
                    </div>
                    <button class="team-menu-btn" type="button" aria-label="More options">
                        <span></span>
                        <span></span>
                        <span></span>
                    </button>
                </header>

                <div class="team-members">
                    <div class="member-row">
                        <div class="member-info-with-avatar">
                            <img src="assets/images/avatar1.png" alt="Mahsa Dinani" class="member-avatar" />
                            <div class="member-info">
                                <p class="member-name">Mahsa Dinani</p>
                                <p class="member-role">Backend Developer</p>
                            </div>
                        </div>
                        <span class="member-badge member-badge-admin">Admin</span>
                    </div>

                    <div class="member-row">
                        <div class="member-info-with-avatar">
                            <img src="assets/images/avatar2.png" alt="Mahsa Dinani" class="member-avatar" />
                            <div class="member-info">
                                <p class="member-name">Mahsa Dinani</p>
                                <p class="member-role">Backend Developer</p>
                            </div>
                        </div>
                        <span class="member-badge member-badge-admin">Admin</span>
                    </div>

                    <div class="member-row">
                        <div class="member-info-with-avatar">
                            <img src="assets/images/avatar3.png" alt="Mahsa Dinani" class="member-avatar" />
                            <div class="member-info">
                                <p class="member-name">Elahe Mahmudi</p>
                                <p class="member-role">Frontend Developer</p>
                            </div>
                        </div>

                        <span class="member-badge member-badge-member">Member</span>
                    </div>
                </div>

                <div class="team-card-footer">
                    <button class="add-member-btn" type="button">
                        + Add member
                    </button>
                </div>
            </section>

            <!-- Team card 2 -->
            <section class="team-card">
                <header class="team-card-header">
                    <div class="team-card-title">
                        <span class="team-emoji">🎨</span>
                        <div class="team-texts">
                            <h2 class="team-name">Design Team</h2>
                            <p class="team-members-count">2 members</p>
                        </div>
                    </div>
                    <button class="team-menu-btn" type="button" aria-label="More options">
                        <span></span>
                        <span></span>
                        <span></span>
                    </button>
                </header>

                <div class="team-members">
                    <div class="member-row">
                        <div class="member-info-with-avatar">
                            <img src="assets/images/avatar4.png" alt="Mahsa Dinani" class="member-avatar" />
                            <div class="member-info">
                                <p class="member-name">Mehrnaz Osquee</p>
                                <p class="member-role">UI/UX Designer</p>
                            </div>
                        </div>
                        <span class="member-badge member-badge-admin">Admin</span>
                    </div>

                    <div class="member-row">
                        <div class="member-info-with-avatar">
                            <img src="assets/images/avatar5.png" alt="Mahsa Dinani" class="member-avatar" />
                            <div class="member-info">
                                <p class="member-name">Melika Judi</p>
                                <p class="member-role">Creative Director</p>
                            </div>
                        </div>
                        <span class="member-badge member-badge-member">Member</span>
                    </div>
                </div>

                <div class="team-card-footer">
                    <button class="add-member-btn" type="button">
                        + Add member
                    </button>
                </div>
            </section>

            <!-- Team card 3 -->
            <section class="team-card">
                <header class="team-card-header">
                    <div class="team-card-title">
                        <span class="team-emoji">📣</span>
                        <div class="team-texts">
                            <h2 class="team-name">Marketing Team</h2>
                            <p class="team-members-count">3 members</p>
                        </div>
                    </div>
                    <button class="team-menu-btn" type="button" aria-label="More options">
                        <span></span>
                        <span></span>
                        <span></span>
                    </button>
                </header>

                <div class="team-members">
                    <div class="member-row">
                        <div class="member-info-with-avatar">
                            <img src="assets/images/avatar6.png" alt="Mahsa Dinani" class="member-avatar" />
                            <div class="member-info">
                                <p class="member-name">Aylin Noushin</p>
                                <p class="member-role">Marketing Management</p>
                            </div>
                        </div>
                        <span class="member-badge member-badge-admin">Admin</span>
                    </div>

                    <div class="member-row">
                        <div class="member-info-with-avatar">
                            <img src="assets/images/avatar7.png" alt="Mahsa Dinani" class="member-avatar" />
                            <div class="member-info">
                                <p class="member-name">Haniye Salehi</p>
                                <p class="member-role">Content Writer</p>
                            </div>
                        </div>
                        <span class="member-badge member-badge-member">Member</span>
                    </div>

                    <div class="member-row">
                        <div class="member-info-with-avatar">
                            <img src="assets/images/avatar1.png" alt="Mahsa Dinani" class="member-avatar" />
                            <div class="member-info">
                                <p class="member-name">
                                    Sara Rezaie with a very long family name to test truncation
                                </p>
                                <p class="member-role">
                                    Social Media Specialist and Strategy Consultant with long title
                                </p>
                            </div>
                        </div>
                        <span class="member-badge member-badge-member">Member</span>
                    </div>
                </div>

                <div class="team-card-footer">
                    <button class="add-member-btn" type="button">
                        + Add member
                    </button>
                </div>
            </section>
        </div>
    </div>

    <!-- Create Team Modal Structure -->
    <div id="createTeamModal" class="modal-overlay" style="display: none;">
        <div class="modal-content">
            <!-- Logo Circle Placeholder -->
            <div class="modal-logo-section">
                <div class="logo-circle">
                    <span>Logo</span>
                </div>
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

    <asp:HiddenField ID="hfTeamMembers" runat="server" />
    <asp:HiddenField ID="hfTeamName" runat="server" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        function showCreateTeamModal() {
            document.getElementById('createTeamModal').style.display = 'flex';
            document.getElementById('txtNewTeamName').value = '';
            document.getElementById('membersList').innerHTML = '';
            // Add one empty row by default? No, prompt says "when clicked... adds a new row".
            // But usually nice to have one. I'll stick to button adding it or maybe empty start.
            // Prompt: "پایین این ها یک باکس باشد به نام add member ... وقتی روی آن کلیک شد، یک ردیف جدید بسازد"
            // So initially maybe empty.
        }

        function closeCreateTeamModal() {
            document.getElementById('createTeamModal').style.display = 'none';
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
        });
    </script>
</asp:Content>


