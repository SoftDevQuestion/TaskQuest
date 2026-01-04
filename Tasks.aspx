<%@ Page Language="C#" MasterPageFile="~/SideBar.master"  AutoEventWireup="true" CodeBehind="Tasks.aspx.cs" Inherits="TaskQuest.Tasks" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <title>Tasks Page</title>
    <link rel="stylesheet" href="assets/css/tasks.css">
    <link rel="stylesheet" href="assets/css/tasks-modal.css">
    <!-- Using Feather Icons for UI elements -->
    <script src="https://unpkg.com/feather-icons"></script>
</asp:Content>

<asp:Content ID="TasksContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tasks-page-container">
        
        <!-- Page Header -->
        <div class="tasks-header-controls">
            <h1 class="tasks-title">Tasks</h1>
            <button class="sort-btn">
                <i data-feather="filter" style="width: 16px; height: 16px;"></i>
                Sort By : All
            </button>
        </div>

        <!-- Projects Columns Wrapper -->
        <div class="projects-columns-wrapper">
            
            <asp:Repeater ID="rptProjects" runat="server" OnItemDataBound="rptProjects_ItemDataBound">
                <ItemTemplate>
                    <div class="project-column">
                        <div class="project-header-task">
                            <div class="project-header-top">
                                <div class="project-info">
                                    <img src='<%# Eval("ProjectLogo") != DBNull.Value ? Eval("ProjectLogo") : "assets/images/projectTestAvatar.jpg" %>' class="project-icon" alt='<%# Eval("ProjectName") %>' />
                                    <span class="project-name"><%# Eval("ProjectName") %></span>
                                </div>
                            </div>
                            <div class="progress-container">
                                <div class="progress-track">
                                    <div class="progress-fill" style='<%# "width: " + GetProgressPercentage(Container.DataItem) + "%;" %>'></div>
                                </div>
                                <span class="progress-text"><%# GetProgressPercentage(Container.DataItem) %>%</span>
                            </div>
                            <span class="task-stats"><%# Eval("DoneStatusCount") %> Completed / <%# Convert.ToInt32(Eval("ToDoStatusCount")) + Convert.ToInt32(Eval("InProgressStatusCount")) %> Incomplete</span>
                        </div>

                        <div class="task-list">
                            <asp:Repeater ID="rptTasks" runat="server">
                                <ItemTemplate>
                                    <div class="task-card">
                                        <div class="task-content">
                                            <div class="task-checkbox-wrapper">
                                                <input type="checkbox" class="task-checkbox" onchange="toggleTaskStatus(this, <%# Eval("TaskID") %>)" <%# Convert.ToInt32(Eval("StatusId")) == 3 ? "checked" : "" %> />
                                                <span class="task-title" style='<%# Convert.ToInt32(Eval("StatusId")) == 3 ? "text-decoration: line-through; color: #bdbdbd;" : "" %>'><%# Eval("Title") %></span>
                                            </div>
                                            <i data-feather="edit-2" class="edit-task-icon" style="width: 16px; height: 16px;" onclick="openEditTaskModal(<%# Eval("TaskID") %>)"></i>
                                        </div>
                                        <div class="task-meta">
                                            <span class='<%# "status-badge " + GetStatusClass(Eval("StatusId"), Eval("DueDate")) %>'><%# GetStatusText(Eval("StatusId"), Eval("DueDate")) %></span>
                                            
                                            <!-- Assignee Avatar -->
                                            <div class="task-assignee">
                                                <img src='<%# Eval("AvatarPath") != DBNull.Value && !string.IsNullOrEmpty(Eval("AvatarPath").ToString()) ? Eval("AvatarPath") : "assets/images/default-avatar.svg" %>' 
                                                     title='<%# Eval("FullName") %>' 
                                                     class="assignee-avatar-small" 
                                                     style="width: 24px; height: 24px; border-radius: 50%; object-fit: cover;" 
                                                     onerror="this.src='assets/images/default-avatar.svg'" />
                                            </div>

                                            <span class="task-date"><%# Eval("DueDate") != DBNull.Value ? Convert.ToDateTime(Eval("DueDate")).ToString("MMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture) : "" %></span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <button class="new-task-btn" onclick="openCreateTaskModal(<%# Eval("ProjectID") %>); return false;">
                            <i data-feather="plus" style="width: 16px; height: 16px;"></i>
                            New Task
                        </button>
                        <div class="project-footer">
                            <i data-feather="clock" style="width: 12px; height: 12px;"></i>
                            Created: <%# Eval("CreatedAt") != DBNull.Value ? Convert.ToDateTime(Eval("CreatedAt")).ToString("MMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture) : "" %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            
            <asp:Label ID="lblNoProjects" runat="server" Text="No projects found." Visible="false" CssClass="no-projects-msg"></asp:Label>
        </div>
    </div>

    <!-- Create Task Modal -->
    <div id="createTaskModal" class="modal-overlay">
        <div class="modal-content">
            <div class="modal-header">
                <h3 class="modal-title">Create New Task</h3>
                <button class="close-btn" onclick="closeCreateTaskModal(); return false;">
                    <i data-feather="x" style="width: 20px; height: 20px;"></i>
                </button>
            </div>

            <!-- Task Name -->
            <div class="form-group">
                <label class="form-label">Task Name</label>
                <input type="text" id="txtTaskName" class="form-control" placeholder="e.g. Design Social Media Posts">
            </div>

            <!-- Assign to -->
            <div class="form-group">
                <label class="form-label">Assign to</label>
                <div class="custom-select-wrapper" onclick="this.querySelector('.custom-select').classList.toggle('open');">
                    <div class="custom-select">
                        <div class="custom-select__trigger">
                            <div class="trigger-content" id="selectedUserContent">
                                <img src="assets/images/default-avatar.svg" class="option-avatar hidden" style="display:none;" id="selectedUserAvatar">
                                <span id="selectedUserName">Unassigned</span>
                            </div>
                            <div class="arrow"></div>
                        </div>
                        <div class="custom-options">
                            <span class="custom-option selected" onclick="selectUser(this, '', 'Unassigned', '')">
                                <img src="assets/images/default-avatar.svg" class="option-avatar">
                                <span class="option-name">Unassigned</span>
                            </span>
                            <asp:Repeater ID="rptAssignees" runat="server">
                                <ItemTemplate>
                                    <span class="custom-option" onclick="selectUser(this, '<%# Eval("Username") %>', '<%# Eval("FullName") %>', '<%# Eval("AvatarPath") != DBNull.Value && !string.IsNullOrEmpty(Eval("AvatarPath").ToString()) ? Eval("AvatarPath") : "assets/images/default-avatar.svg" %>')">
                                        <img src='<%# Eval("AvatarPath") != DBNull.Value && !string.IsNullOrEmpty(Eval("AvatarPath").ToString()) ? Eval("AvatarPath") : "assets/images/default-avatar.svg" %>' class="option-avatar" onerror="this.src='assets/images/default-avatar.svg'">
                                        <div class="option-info">
                                            <span class="option-name"><%# Eval("FullName") %></span>
                                            <span class="option-role"><%# Eval("Username") %></span>
                                        </div>
                                    </span>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Due Date -->
            <div class="form-group">
                <label class="form-label">Due Date</label>
                <input type="date" id="txtDueDate" class="form-control">
                <div id="dateError" class="date-error" style="display: none;">
                    <i data-feather="alert-triangle" style="width: 12px; height: 12px;"></i>
                    This due date is in the past
                </div>
            </div>

            <!-- Footer -->
            <div class="modal-footer">
                <button class="btn-cancel" onclick="closeCreateTaskModal(); return false;">Cancel</button>
                <button class="btn-create" onclick="createTask(); return false;">Create Task</button>
            </div>
        </div>
    </div>

    <!-- Edit Task Modal -->
    <div id="editTaskModal" class="modal-overlay">
        <div class="modal-content">
            <div class="modal-header">
                <h3 class="modal-title">Edit Task</h3>
                <button class="close-btn" onclick="closeEditTaskModal(); return false;">
                    <i data-feather="x" style="width: 20px; height: 20px;"></i>
                </button>
            </div>

            <!-- Task Name -->
            <div class="form-group">
                <label class="form-label">Task Name</label>
                <input type="text" id="txtEditTaskName" class="form-control">
            </div>

            <!-- Assign to -->
            <div class="form-group">
                <label class="form-label">Assign to</label>
                <div class="custom-select-wrapper" onclick="this.querySelector('.custom-select').classList.toggle('open');">
                    <div class="custom-select">
                        <div class="custom-select__trigger">
                            <div class="trigger-content" id="selectedUserContentEdit">
                                <img src="assets/images/default-avatar.svg" class="option-avatar hidden" style="display:none;" id="selectedUserAvatarEdit">
                                <span id="selectedUserNameEdit">Unassigned</span>
                            </div>
                            <div class="arrow"></div>
                        </div>
                        <div class="custom-options">
                            <span class="custom-option selected" onclick="selectUserEdit(this, '', 'Unassigned', '')">
                                <img src="assets/images/default-avatar.svg" class="option-avatar">
                                <span class="option-name">Unassigned</span>
                            </span>
                            <asp:Repeater ID="rptAssigneesEdit" runat="server">
                                <ItemTemplate>
                                    <span class="custom-option" onclick="selectUserEdit(this, '<%# Eval("Username") %>', '<%# Eval("FullName") %>', '<%# Eval("AvatarPath") != DBNull.Value && !string.IsNullOrEmpty(Eval("AvatarPath").ToString()) ? Eval("AvatarPath") : "assets/images/default-avatar.svg" %>')">
                                        <img src='<%# Eval("AvatarPath") != DBNull.Value && !string.IsNullOrEmpty(Eval("AvatarPath").ToString()) ? Eval("AvatarPath") : "assets/images/default-avatar.svg" %>' class="option-avatar" onerror="this.src='assets/images/default-avatar.svg'">
                                        <div class="option-info">
                                            <span class="option-name"><%# Eval("FullName") %></span>
                                            <span class="option-role"><%# Eval("Username") %></span>
                                        </div>
                                    </span>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Due Date -->
            <div class="form-group">
                <label class="form-label">Due Date</label>
                <input type="date" id="txtEditDueDate" class="form-control">
            </div>

            <!-- Footer -->
            <div class="modal-footer">
                <button class="btn-cancel" onclick="closeEditTaskModal(); return false;">Cancel</button>
                <button class="btn-create" onclick="updateTask(); return false;">Edit Task</button>
            </div>
        </div>
    </div>

    <!-- Hidden field to store current Project ID -->
    <input type="hidden" id="hfCurrentProjectId" />
    <input type="hidden" id="hfSelectedAssignee" />
    <input type="hidden" id="hfEditingTaskId" />
    <input type="hidden" id="hfSelectedAssigneeEdit" />

    <script>
        function openEditTaskModal(taskId) {
            document.getElementById('hfEditingTaskId').value = taskId;
            
            // Fetch task details
            PageMethods.GetTaskDetails(taskId, function(result) {
                if (result) {
                    if (result.Error) {
                        alert(result.Error);
                        return;
                    }
                    
                    document.getElementById('txtEditTaskName').value = result.Title;
                    document.getElementById('txtEditDueDate').value = result.DueDate;
                    
                    // Set Assignee
                    selectUserEdit(null, result.AssigneeUsername, result.AssigneeFullName, result.AssigneeAvatar);
                    
                    const modal = document.getElementById('editTaskModal');
                    modal.classList.add('show');
                    feather.replace();
                }
            }, function(err) {
                console.error(err);
                alert("Error fetching task details.");
            });
        }

        function closeEditTaskModal() {
            document.getElementById('editTaskModal').classList.remove('show');
        }

        function selectUserEdit(element, userId, userName, userAvatar) {
            document.getElementById('selectedUserNameEdit').innerText = userName;
            const avatarImg = document.getElementById('selectedUserAvatarEdit');
            if (userAvatar) {
                avatarImg.src = userAvatar;
                avatarImg.style.display = 'block';
            } else {
                avatarImg.style.display = 'none';
            }
            
            document.getElementById('hfSelectedAssigneeEdit').value = userId;

            // Update selection UI
            const wrapper = document.querySelector('#editTaskModal .custom-options');
            if(wrapper) {
                const options = wrapper.querySelectorAll('.custom-option');
                options.forEach(opt => opt.classList.remove('selected'));
                if (element) {
                    element.classList.add('selected');
                }
            }
        }

        function updateTask() {
            const taskId = document.getElementById('hfEditingTaskId').value;
            const title = document.getElementById('txtEditTaskName').value;
            const dueDate = document.getElementById('txtEditDueDate').value;
            const assignee = document.getElementById('hfSelectedAssigneeEdit').value;

            PageMethods.UpdateTask(taskId, title, dueDate, assignee, function(response) {
                if (response === "Success") {
                    location.reload();
                } else {
                    alert(response);
                }
            }, function(err) {
                console.error(err);
                alert("Error updating task.");
            });
        }

        function openCreateTaskModal(projectId) {
            document.getElementById('hfCurrentProjectId').value = projectId;
            const modal = document.getElementById('createTaskModal');
            modal.classList.add('show');
            feather.replace(); // Refresh icons inside modal
        }

        function closeCreateTaskModal() {
            const modal = document.getElementById('createTaskModal');
            modal.classList.remove('show');
            
            // Reset form
            document.getElementById('txtTaskName').value = '';
            document.getElementById('txtDueDate').value = '';
            // Reset selection
            selectUser(null, '', 'Unassigned', '');
        }

        function selectUser(element, userId, userName, userAvatar) {
            // Update Trigger
            document.getElementById('selectedUserName').innerText = userName;
            const avatarImg = document.getElementById('selectedUserAvatar');
            if (userAvatar) {
                avatarImg.src = userAvatar;
                avatarImg.style.display = 'block';
            } else {
                avatarImg.style.display = 'none';
            }
            
            // Set hidden field
            document.getElementById('hfSelectedAssignee').value = userId;

            // Update Selected State in Options
            const options = document.querySelectorAll('.custom-option');
            options.forEach(opt => opt.classList.remove('selected'));
            if (element) {
                element.classList.add('selected');
            } else {
                // Select first one (Unassigned)
                if (options.length > 0) options[0].classList.add('selected');
            }
        }

        function createTask() {
            const projectId = document.getElementById('hfCurrentProjectId').value;
            const taskName = document.getElementById('txtTaskName').value;
            const dueDate = document.getElementById('txtDueDate').value;
            const assignee = document.getElementById('hfSelectedAssignee').value;

            if (!taskName) {
                alert('Please enter a task name');
                return;
            }

            fetch('Tasks.aspx/CreateNewTask', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ projectId: projectId, title: taskName, dueDate: dueDate, assigneeUsername: assignee })
            })
            .then(res => res.json())
            .then(data => {
                if(data.d === 'Success') {
                    closeCreateTaskModal();
                    window.location.reload();
                } else {
                    alert(data.d);
                }
            })
            .catch(err => {
                console.error(err);
                alert('An error occurred while creating the task.');
            });
        }

        function toggleTaskStatus(checkbox, taskId) {
            const isDone = checkbox.checked;
            
            fetch('Tasks.aspx/ToggleTaskStatus', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ taskId: taskId, isDone: isDone })
            })
            .then(res => res.json())
            .then(data => {
                if(data.d === 'Success') {
                    window.location.reload();
                } else {
                    console.error(data.d);
                    checkbox.checked = !isDone;
                }
            })
            .catch(err => {
                console.error(err);
                checkbox.checked = !isDone;
            });
        }

        // Close modal when clicking outside
        window.onclick = function(event) {
            const modal = document.getElementById('createTaskModal');
            if (event.target == modal) {
                closeCreateTaskModal();
            }
        }
        
        // Date validation
        document.getElementById('txtDueDate').addEventListener('change', function() {
            const selectedDate = new Date(this.value);
            const today = new Date();
            today.setHours(0,0,0,0);
            
            const errorMsg = document.getElementById('dateError');
            if (selectedDate < today) {
                errorMsg.style.display = 'flex';
                feather.replace();
            } else {
                errorMsg.style.display = 'none';
            }
        });

        feather.replace();
    </script>
</asp:Content>