# Changes Log

## 2026-01-04 - Security & Cascade Delete Update

### 1. Database Changes
- **Table `Team`**: Added `CreatorUsername` column (NVARCHAR(50)) to track team ownership.
- **Data Migration**: Backfilled `CreatorUsername` for existing teams (defaulted to 'admin').
- **Stored Procedures**:
  - Created `sp_DeleteUserCascade`: Handles deletion of users along with their created teams, projects, and tasks in a single transaction.

### 2. Backend Logic Updates (Access Control)
- **Strict Ownership Model**: Access is now restricted based on creation/ownership rather than just membership.
- **`Teams.aspx.cs`**:
  - `LoadTeams()` now filters by `CreatorUsername = @CurrentUser`.
  - Users can only see teams they explicitly created.
- **`Projects.aspx.cs`**:
  - `LoadProjects()` now filters projects based on:
    1. Project created by current user.
    2. Project assigned to a team created by current user.
  - Users cannot see projects of teams they are just members of (unless they own the team).
- **`SideBar.Master.cs`**:
  - Verified logic matches the new ownership requirements (Recent Teams/Projects filters by `CreatorUsername`).

### 3. Testing
- **Cascade Delete Test**: `RunTest.ps1` verifies that deleting a user removes all associated data (Teams, Projects, Tasks).
- **Security Access Test**: `TestSecurityQuery.ps1` verifies that database queries correctly filter teams and projects based on ownership, excluding items where the user is merely a member.
