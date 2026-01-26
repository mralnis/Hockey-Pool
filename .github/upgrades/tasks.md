# .NET 10 Upgrade Execution Tasks

## Metadata
- **Scenario:** .NET 9 to .NET 10 Upgrade
- **Strategy:** All-At-Once
- **Solution:** d:\Repo\Hockey-Pool\HockeyPool.sln
- **Projects:** 2
- **Target Framework:** net10.0
- **Branch:** upgrade-to-NET10

## Task List

### [?] TASK-001: Prerequisites Verification
**Description:** Verify system prerequisites before starting upgrade

**Actions:**
- [ ] (1) Verify .NET 10 SDK is installed (`dotnet --version` should show 10.x.x)
- [ ] (2) Check for global.json file in solution root and verify compatibility
- [ ] (3) Ensure we're on upgrade-to-NET10 branch (create if needed)
- [ ] (4) Verify no pending changes (commit or stash if needed)

**Verification:** All prerequisites met, SDK available, branch ready

---

### [ ] TASK-002: Update Infrastructure Project
**Description:** Update Infrastructure\HockeyPool.Infrastructure.csproj to .NET 10

**Actions:**
- [ ] (1) Update TargetFramework from net9.0 to net10.0
- [ ] (2) Update Microsoft.AspNetCore.Identity.EntityFrameworkCore from 9.0.4 to 10.0.2
- [ ] (3) Update Microsoft.EntityFrameworkCore.SqlServer from 9.0.4 to 10.0.2
- [ ] (4) Update Microsoft.EntityFrameworkCore.Tools from 9.0.4 to 10.0.2
- [ ] (5) Update Microsoft.Extensions.Hosting.Abstractions from 9.0.4 to 10.0.2
- [ ] (6) Update Microsoft.Extensions.Identity.Stores from 9.0.4 to 10.0.2
- [ ] (7) Save project file
- [ ] (8) Run `dotnet restore Infrastructure\HockeyPool.Infrastructure.csproj`
- [ ] (9) Run `dotnet build Infrastructure\HockeyPool.Infrastructure.csproj`

**Verification:** Infrastructure project builds successfully with 0 errors, 0 warnings

---

### [ ] TASK-003: Update HockeyPool Application Project
**Description:** Update HockeyPool\HockeyPool.csproj to .NET 10

**Actions:**
- [ ] (1) Update TargetFramework from net9.0 to net10.0
- [ ] (2) Update Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore from 9.0.4 to 10.0.2
- [ ] (3) Update Microsoft.AspNetCore.Identity.EntityFrameworkCore from 9.0.4 to 10.0.2
- [ ] (4) Update Microsoft.EntityFrameworkCore.Design from 9.0.4 to 10.0.2
- [ ] (5) Update Microsoft.EntityFrameworkCore.Sqlite from 9.0.4 to 10.0.2
- [ ] (6) Remove deprecated SQLitePCLRaw.lib.e_sqlite3 package reference (if explicit)
- [ ] (7) Verify MudBlazor remains at 8.6.0 (no change needed)
- [ ] (8) Save project file
- [ ] (9) Run `dotnet restore HockeyPool\HockeyPool.csproj`

**Verification:** Restore completes successfully, deprecated package removed

---

### [ ] TASK-004: Fix HttpPipelineConfiguration Namespace Issues
**Description:** Fix middleware extension method namespaces in HttpPipelineConfiguration.cs

**Actions:**
- [ ] (1) Open HockeyPool\Configuration\HttpPipelineConfiguration.cs
- [ ] (2) Add namespace: `using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;` at top of file
- [ ] (3) Verify UseMigrationsEndPoint() call at line 8 is now in scope
- [ ] (4) Review UseExceptionHandler() call at line 12 for behavioral changes
- [ ] (5) Save file

**Verification:** File compiles without namespace errors

---

### [ ] TASK-005: Fix DatabaseConfiguration Namespace Issues
**Description:** Fix database developer page exception filter namespace in DatabaseConfiguration.cs

**Actions:**
- [ ] (1) Open HockeyPool\Configuration\DatabaseConfiguration.cs
- [ ] (2) Add namespace: `using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;` at top of file
- [ ] (3) Verify AddDatabaseDeveloperPageExceptionFilter() call at line 15 is now in scope
- [ ] (4) Save file

**Verification:** File compiles without namespace errors

---

### [ ] TASK-006: Fix AuthenticationConfiguration Namespace Issues
**Description:** Fix Identity Entity Framework stores namespace in AuthenticationConfiguration.cs

**Actions:**
- [ ] (1) Open HockeyPool\Configuration\AuthenticationConfiguration.cs
- [ ] (2) Add namespace: `using Microsoft.AspNetCore.Identity.EntityFrameworkCore;` at top of file
- [ ] (3) Verify AddEntityFrameworkStores<ApplicationDbContext>() call at line 35 is now in scope
- [ ] (4) Save file

**Verification:** File compiles without namespace errors

---

### [ ] TASK-007: Fix TimeSpan.FromMinutes Signature
**Description:** Fix TimeSpan.FromMinutes parameter in IdentityRevalidatingAuthenticationStateProvider.cs

**Actions:**
- [ ] (1) Open HockeyPool\Components\Account\IdentityRevalidatingAuthenticationStateProvider.cs
- [ ] (2) Locate TimeSpan.FromMinutes(30) at line 16
- [ ] (3) Change to TimeSpan.FromMinutes(30L) to use explicit long literal
- [ ] (4) Save file

**Verification:** File compiles without signature errors

---

### [ ] TASK-008: Build Entire Solution
**Description:** Build complete solution to verify all changes

**Actions:**
- [ ] (1) Run `dotnet clean` on solution
- [ ] (2) Run `dotnet restore` on solution
- [ ] (3) Run `dotnet build HockeyPool.sln`
- [ ] (4) Verify build output shows 0 errors, 0 warnings
- [ ] (5) Check for any obsolete API warnings

**Verification:** Solution builds successfully with 0 errors, 0 warnings

---

### [ ] TASK-009: Verify Package Security and Quality
**Description:** Verify no security vulnerabilities or deprecated packages

**Actions:**
- [ ] (1) Run `dotnet list package --vulnerable` on solution
- [ ] (2) Verify no vulnerable packages found
- [ ] (3) Run `dotnet list package --deprecated` on solution
- [ ] (4) Verify no deprecated packages found (SQLitePCLRaw should be gone)
- [ ] (5) Run `dotnet list package` to confirm all versions

**Verification:** No vulnerabilities, no deprecated packages, all versions correct

---

### [ ] TASK-010: Test Application Startup
**Description:** Verify application starts successfully

**Actions:**
- [ ] (1) Run application: `dotnet run --project HockeyPool\HockeyPool.csproj`
- [ ] (2) Verify application starts without runtime errors
- [ ] (3) Check console for any startup errors or warnings
- [ ] (4) Verify database connection establishes (SQLite)
- [ ] (5) Stop application

**Verification:** Application starts successfully, no runtime errors, database accessible

---

### [ ] TASK-011: Manual Smoke Testing - Authentication
**Description:** Test authentication and authorization features

**Actions:**
- [ ] (1) Start application
- [ ] (2) Navigate to login page
- [ ] (3) Test login with valid credentials
- [ ] (4) Verify authentication state persists
- [ ] (5) Test logout functionality
- [ ] (6) Test registration flow (including email confirmation URL construction)
- [ ] (7) Verify admin role authorization (if applicable)

**Verification:** All authentication workflows function correctly

---

### [ ] TASK-012: Manual Smoke Testing - Core Features
**Description:** Test main application features

**Actions:**
- [ ] (1) Navigate to Betting Overview page - verify loads correctly
- [ ] (2) Navigate to Betting History page - verify data displays
- [ ] (3) Navigate to Winners page - verify content loads
- [ ] (4) Test all navigation links and routing
- [ ] (5) Test redirect scenarios with return URLs (System.Uri behavioral changes)
- [ ] (6) Verify no console errors in browser developer tools

**Verification:** All core features functional, no JavaScript errors, redirects work correctly

---

### [ ] TASK-013: Run Tests (If Present)
**Description:** Execute unit and integration tests

**Actions:**
- [ ] (1) Run `dotnet test --no-build` on solution
- [ ] (2) Verify all tests pass
- [ ] (3) Check test results for any failures or skips
- [ ] (4) If tests fail, investigate and fix (may need test updates for behavioral changes)

**Verification:** All tests pass, no test failures

**Note:** If no tests exist, mark as complete

---

### [ ] TASK-014: Commit All Changes
**Description:** Commit upgrade changes to upgrade-to-NET10 branch

**Actions:**
- [ ] (1) Review all changed files with `git status`
- [ ] (2) Stage all changes: `git add .`
- [ ] (3) Commit with message:
  ```
  Upgrade solution from .NET 9 to .NET 10 LTS

  - Updated both projects from net9.0 to net10.0
  - Upgraded 8 NuGet packages (EF Core 9.0.4 ? 10.0.2)
  - Fixed 4 namespace issues in configuration files
  - Fixed TimeSpan.FromMinutes signature in IdentityRevalidatingAuthenticationStateProvider
  - Removed deprecated SQLitePCLRaw.lib.e_sqlite3 package
  - All builds successful, all tests pass, application functional

  Breaking changes tested:
  - System.Uri behavior (email confirmation, redirects)
  - Exception handling middleware
  - Identity authentication flow
  - SQLite database operations
  ```
- [ ] (4) Verify commit successful

**Verification:** Changes committed to upgrade-to-NET10 branch

---

### [ ] TASK-015: Final Verification and Documentation
**Description:** Final checks and update documentation

**Actions:**
- [ ] (1) Run all verification commands from Success Criteria section
- [ ] (2) Verify all success criteria met
- [ ] (3) Update README.md if needed (document .NET 10 requirement)
- [ ] (4) Document any issues encountered and resolutions
- [ ] (5) Prepare summary of changes for team

**Verification:** All success criteria met, documentation updated

---

## Progress Dashboard

**Overall Progress:** 0/15 tasks complete (0%)

**Phase Status:**
- Prerequisites: ? Pending
- Project Updates: ? Pending
- Code Fixes: ? Pending
- Build & Test: ? Pending
- Commit: ? Pending
- Final Verification: ? Pending

**Last Updated:** [Not started]

---

## Execution Notes

### Critical Reminders
- All changes happen atomically (All-At-Once strategy)
- Infrastructure must build before HockeyPool
- Stop immediately if any task verification fails
- Test System.Uri behavioral changes thoroughly

### Key Files to Modify
1. Infrastructure\HockeyPool.Infrastructure.csproj
2. HockeyPool\HockeyPool.csproj
3. HockeyPool\Configuration\HttpPipelineConfiguration.cs
4. HockeyPool\Configuration\DatabaseConfiguration.cs
5. HockeyPool\Configuration\AuthenticationConfiguration.cs
6. HockeyPool\Components\Account\IdentityRevalidatingAuthenticationStateProvider.cs

### Expected Outcomes
- 2 project files updated to net10.0
- 8 packages upgraded to 10.0.2
- 1 deprecated package removed
- 3 namespace additions
- 1 TimeSpan signature fix
- Solution builds with 0 errors, 0 warnings
- All tests pass
- Application runs successfully
