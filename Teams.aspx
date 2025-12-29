<%@ Page Language="C#" MasterPageFile="~/SideBar.master" AutoEventWireup="true" CodeBehind="Teams.aspx.cs" Inherits="TaskQuest.Teams" %>



<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <title>Teams Page</title>
    <link rel="stylesheet" href="assets/css/teams.css">
</asp:Content>




<asp:Content ID="TeamsContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- TeamCards.aspx (فقط بخش کارت‌ها) -->
    <div class="teams-page">
        <h1 class="page-title">Team Management</h1>

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
</asp:Content>


