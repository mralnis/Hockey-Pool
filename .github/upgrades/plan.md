# .NET 10 Upgrade Plan - Hockey Pool Solution

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
  - [Infrastructure\HockeyPool.Infrastructure.csproj](#infrastructurehockeypoolinfrastructurecsproj)
  - [HockeyPool\HockeyPool.csproj](#hockeypoolhockeypoolcsproj)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description
Upgrade the Hockey Pool Blazor application from **.NET 9.0** to **.NET 10.0 (Long Term Support)**. This upgrade targets a 2-project solution consisting of a Blazor Server web application and a supporting infrastructure class library.

### Scope
**Projects Affected:** 2
- **HockeyPool\HockeyPool.csproj** - Blazor Server application (net9.0 ? net10.0)
- **Infrastructure\HockeyPool.Infrastructure.csproj** - Class library with Entity Framework Core (net9.0 ? net10.0)

**Current State:**
- Both projects targeting .NET 9.0
- Using Entity Framework Core 9.0.4
- Using ASP.NET Core Identity with EF Core 9.0.4
- Using MudBlazor 8.6.0 (compatible with .NET 10)
- Using deprecated SQLitePCLRaw.lib.e_sqlite3 2.1.11

### Target State
- Both projects targeting .NET 10.0
- Entity Framework Core packages upgraded to 10.0.2
- ASP.NET Core Identity packages upgraded to 10.0.2
- SQLitePCLRaw.lib.e_sqlite3 deprecated package addressed
- All API compatibility issues resolved (7 source incompatible, 6 behavioral changes)

### Selected Strategy
**All-At-Once Strategy** - All projects upgraded simultaneously in a single atomic operation.

**Rationale:**
- **Small solution** (2 projects) ideal for coordinated upgrade
- **Simple dependency structure** (linear: HockeyPool ? Infrastructure, depth = 1)
- **Low complexity** (3,288 total LOC, only 13+ LOC estimated to modify = 0.4%)
- **No blocking issues** (no security vulnerabilities, no circular dependencies)
- **Clear package upgrade path** (all Microsoft EF Core packages have .NET 10 versions)
- **Homogeneous codebase** (both projects C# Blazor/EF Core patterns)

This approach enables the fastest completion time with minimal coordination overhead.

### Complexity Assessment

**Discovered Metrics:**
- Total Projects: 2
- Total NuGet Packages: 10 (9 need upgrade)
- Total Code Files: 38
- Code Files with API Issues: 8
- Total Lines of Code: 3,288
- Estimated LOC to Modify: 13+ (0.4% of codebase)
- Dependency Depth: 1 (Infrastructure ? HockeyPool)
- High-Risk Projects: 0

**Complexity Classification: Simple**

**API Compatibility:**
- ?? Source Incompatible: 7 issues (require recompilation, potential API changes)
- ?? Behavioral Changes: 6 issues (require runtime testing)
- ? Compatible APIs: 15,896 (99.9% compatibility)

**Package Compatibility:**
- ? Compatible: 1 package (MudBlazor)
- ?? Upgrade Recommended: 8 packages (Microsoft EF Core ecosystem)
- ?? Deprecated: 1 package (SQLitePCLRaw.lib.e_sqlite3)

### Critical Issues
1. **Deprecated Package**: SQLitePCLRaw.lib.e_sqlite3 2.1.11 is deprecated - requires replacement or removal
2. **Source Incompatible APIs**: 7 APIs require code changes (mostly ASP.NET Core middleware extensions)
3. **Behavioral Changes**: 6 APIs with behavior changes (primarily System.Uri and exception handling)

### Recommended Approach
**All-At-Once atomic upgrade** in a single coordinated operation:
1. Update both project files simultaneously (net9.0 ? net10.0)
2. Update all NuGet packages in one batch
3. Address deprecated package
4. Fix all API compatibility issues
5. Build and test solution

**Estimated Effort:** Low complexity - single atomic upgrade phase

### Iteration Strategy
**Fast Batch Approach** (2-3 detail iterations):
- Both projects are low complexity with clear upgrade paths
- All project details will be filled in 1-2 iterations
- Simple dependency structure requires no phase-based separation

---

## Migration Strategy

### Approach Selection

**Selected Strategy: All-At-Once**

**Justification:**
The All-At-Once strategy is optimal for this solution based on the following factors:

1. **Solution Size** ?
   - Only 2 projects (well below 30-project threshold)
   - 3,288 total LOC (small codebase)
   - Simple, linear dependency structure

2. **Current Framework Uniformity** ?
   - Both projects currently on .NET 9.0
   - Both projects targeting .NET 10.0
   - Homogeneous technology stack (EF Core, ASP.NET Core Identity)

3. **Package Compatibility** ?
   - All required packages have .NET 10 versions available
   - Assessment shows clear upgrade path (9.0.4 ? 10.0.2)
   - No blocking compatibility issues

4. **Risk Profile** ?
   - Low complexity (0.4% code impact)
   - No security vulnerabilities
   - No circular dependencies
   - 99.9% API compatibility

5. **Development Efficiency** ?
   - Fastest completion time
   - No multi-targeting complexity
   - Single testing phase
   - Minimal coordination overhead

**Alternative Considered: Incremental Migration**
- Rejected: Unnecessary overhead for 2-project solution
- Would add multi-targeting complexity without meaningful risk reduction
- Would require intermediate testing phases that provide limited value

### All-At-Once Strategy Implementation

**Core Principle:** Atomic, simultaneous upgrade of all projects

**Execution Characteristics:**
- All project files updated to net10.0 in single operation
- All NuGet packages updated in single operation
- Single solution build to identify all compilation errors
- All errors fixed in coordinated manner
- Single comprehensive testing phase

**No Intermediate States:**
- Solution either fully on .NET 9 or fully on .NET 10
- No projects remain on old framework during migration
- All developers work with same framework version

### Dependency-Based Ordering Rationale

While the All-At-Once strategy updates all projects simultaneously, the logical dependency order still matters for **error resolution**:

**Logical Order (for error fixing):**
1. **Infrastructure\HockeyPool.Infrastructure.csproj** (dependency, no dependants within solution)
2. **HockeyPool\HockeyPool.csproj** (depends on Infrastructure)

**Rationale:**
- Infrastructure is a leaf node (no dependencies within solution)
- HockeyPool depends on Infrastructure's public APIs
- Fixing Infrastructure errors first ensures HockeyPool can compile against correct APIs
- However, both projects' framework/packages update simultaneously

### Execution Sequence

**Phase 0: Prerequisites** (if needed)
- Verify .NET 10 SDK installed on development machine
- Check for and update global.json if present

**Phase 1: Atomic Upgrade** (all operations performed together)
1. Update TargetFramework in both project files (net9.0 ? net10.0)
2. Update all NuGet package references across both projects (9.0.4 ? 10.0.2)
3. Address deprecated SQLitePCLRaw.lib.e_sqlite3 package
4. Restore dependencies (`dotnet restore`)
5. Build entire solution (`dotnet build`)
6. Fix all compilation errors discovered (prioritize Infrastructure first)
7. Rebuild solution to verify fixes
8. Verify 0 errors, 0 warnings

**Phase 2: Testing & Validation**
1. Run all unit tests (if present)
2. Run integration tests (if present)
3. Manual smoke testing of application
4. Validate runtime behavior for APIs with behavioral changes

### Risk Management Integration

**Risk Mitigation Through All-At-Once:**
- ? **Speed advantage** reduces risk exposure window
- ? **No multi-targeting** eliminates cross-version compatibility bugs
- ? **Single test cycle** ensures comprehensive validation

**Risk Factors Requiring Attention:**
- ?? Higher initial compilation error count (expected, manageable)
- ?? All developers must upgrade simultaneously
- ?? Requires coordinated deployment

**Mitigation Strategies:**
- Create upgrade branch (upgrade-to-NET10) to isolate changes
- Comprehensive testing before merging to main branch
- Document all API changes for team awareness
- Keep detailed log of all fixes applied

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The Hockey Pool solution has a simple, linear dependency structure:

```
HockeyPool.csproj (Blazor Application)
    ??? HockeyPool.Infrastructure.csproj (Class Library)
```

**Dependency Depth:** 1 (single level)  
**Circular Dependencies:** None  
**External Dependencies:** 10 NuGet packages

### Project Groupings by Migration Phase

Given the All-At-Once strategy, both projects are upgraded in a **single atomic phase**:

**Phase 1: Atomic Upgrade** (All Projects Simultaneously)
1. **Infrastructure\HockeyPool.Infrastructure.csproj** - Class library (must logically be ready first as it's a dependency)
2. **HockeyPool\HockeyPool.csproj** - Blazor application (depends on Infrastructure)

**Note:** While Infrastructure is a dependency of HockeyPool, the All-At-Once strategy means we update target frameworks and packages for both projects at the same time, then build the entire solution together to discover and fix all compilation errors in one pass.

### Critical Path Identification

**Critical Path:** Infrastructure ? HockeyPool

**Key Dependencies:**
- HockeyPool.csproj directly references HockeyPool.Infrastructure.csproj
- Both projects share common EF Core and Identity packages
- Both projects must target compatible framework versions

**Migration Order Consideration:**
While traditional incremental approaches would upgrade Infrastructure first (bottom-up), the All-At-Once strategy updates both projects simultaneously. However, when fixing compilation errors, we should prioritize Infrastructure fixes first since HockeyPool depends on it.

### Parallel Execution Opportunities

**Not Applicable for All-At-Once Strategy**

Since this strategy performs a single atomic upgrade, there's no parallelization of project upgrades. All changes happen together:
- Both project files updated in same operation
- All package references updated in same operation
- Solution built once to identify all errors
- All errors fixed in coordinated manner

### Package Dependencies

**Shared Packages** (both projects):
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 9.0.4 ? 10.0.2

**Infrastructure-Specific Packages:**
- Microsoft.EntityFrameworkCore.SqlServer 9.0.4 ? 10.0.2
- Microsoft.EntityFrameworkCore.Tools 9.0.4 ? 10.0.2
- Microsoft.Extensions.Hosting.Abstractions 9.0.4 ? 10.0.2
- Microsoft.Extensions.Identity.Stores 9.0.4 ? 10.0.2

**HockeyPool-Specific Packages:**
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore 9.0.4 ? 10.0.2
- Microsoft.EntityFrameworkCore.Design 9.0.4 ? 10.0.2
- Microsoft.EntityFrameworkCore.Sqlite 9.0.4 ? 10.0.2
- MudBlazor 8.6.0 (already compatible, no upgrade needed)
- SQLitePCLRaw.lib.e_sqlite3 2.1.11 (deprecated - requires action)

### Circular Dependency Analysis

**Status:** ? No circular dependencies detected

The solution has a clean, unidirectional dependency structure making the upgrade straightforward.

---

## Project-by-Project Plans

## Project-by-Project Plans

### Infrastructure\HockeyPool.Infrastructure.csproj

#### Current State

- **Target Framework:** net9.0
- **Project Type:** Class Library (SDK-style)
- **Lines of Code:** 2,829
- **Files:** 23
- **Files with API Issues:** 1
- **Dependencies:** 0 (within solution)
- **Dependants:** 1 (HockeyPool.csproj)
- **Risk Level:** ?? LOW

**Current NuGet Packages:**
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 9.0.4
- Microsoft.EntityFrameworkCore.SqlServer 9.0.4
- Microsoft.EntityFrameworkCore.Tools 9.0.4
- Microsoft.Extensions.Hosting.Abstractions 9.0.4
- Microsoft.Extensions.Identity.Stores 9.0.4

**API Compatibility:**
- Binary Incompatible: 0
- Source Incompatible: 0
- Behavioral Changes: 0
- Compatible: 3,777 (100%)

#### Target State

- **Target Framework:** net10.0
- **Updated Packages:** All 5 packages upgraded to version 10.0.2

#### Migration Steps

##### 1. Prerequisites

? **None** - Infrastructure is a leaf node with no solution dependencies

##### 2. Framework Update

**File:** `Infrastructure\HockeyPool.Infrastructure.csproj`

**Change Required:**
```xml
<!-- Current -->
<TargetFramework>net9.0</TargetFramework>

<!-- Updated -->
<TargetFramework>net10.0</TargetFramework>
```

##### 3. Package Updates

Update all Entity Framework Core and Identity packages:

| Package | Current Version | Target Version | Reason |
|---------|----------------|----------------|--------|
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.4 | 10.0.2 | .NET 10 compatibility |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.4 | 10.0.2 | .NET 10 compatibility |
| Microsoft.EntityFrameworkCore.Tools | 9.0.4 | 10.0.2 | .NET 10 compatibility |
| Microsoft.Extensions.Hosting.Abstractions | 9.0.4 | 10.0.2 | .NET 10 compatibility |
| Microsoft.Extensions.Identity.Stores | 9.0.4 | 10.0.2 | .NET 10 compatibility |

**Update in Project File:**
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.2" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.2" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.2">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="Microsoft.Extensions.Hosting.Abstractions" Version="10.0.2" />
  <PackageReference Include="Microsoft.Extensions.Identity.Stores" Version="10.0.2" />
</ItemGroup>
```

##### 4. Expected Breaking Changes

**Status:** ? **NONE**

The assessment indicates **0 API compatibility issues** for this project. All 3,777 analyzed APIs are fully compatible with .NET 10.

**Why No Breaking Changes:**
- Infrastructure project is a class library containing Entity Framework models and data access
- EF Core 10.0.2 maintains strong backward compatibility
- No ASP.NET Core middleware or application startup code

##### 5. Code Modifications

**Status:** ? **NONE REQUIRED**

No code changes are expected for this project based on the assessment.

**Post-Build Verification:**
- If unexpected compilation errors occur, they will likely be related to:
  - Entity Framework model configurations
  - Identity model extensions
  - Database context configurations

##### 6. Testing Strategy

**Unit Tests:**
- If project has unit tests, run them after build succeeds
- Focus on data access layer tests
- Verify Entity Framework LINQ queries still function correctly

**Integration Tests:**
- Test database context initialization
- Verify migrations still work (if using EF migrations)
- Test Identity user store operations

**Manual Verification:**
- Ensure no warnings appear during build
- Check for any obsolete API warnings
- Verify package restore completes successfully

##### 7. Validation Checklist

- [ ] Project file TargetFramework updated to net10.0
- [ ] All 5 PackageReferences updated to version 10.0.2
- [ ] `dotnet restore` completes without errors
- [ ] Project builds without errors (`dotnet build Infrastructure\HockeyPool.Infrastructure.csproj`)
- [ ] Project builds without warnings
- [ ] No package dependency conflicts
- [ ] Unit tests pass (if present)
- [ ] Integration tests pass (if present)

---

### HockeyPool\HockeyPool.csproj

#### Current State

- **Target Framework:** net9.0
- **Project Type:** Blazor Server Application (SDK-style)
- **Lines of Code:** 459
- **Files:** 76
- **Files with API Issues:** 7
- **Dependencies:** 1 (HockeyPool.Infrastructure.csproj)
- **Dependants:** 0
- **Risk Level:** ?? MEDIUM

**Current NuGet Packages:**
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore 9.0.4
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 9.0.4
- Microsoft.EntityFrameworkCore.Design 9.0.4
- Microsoft.EntityFrameworkCore.Sqlite 9.0.4
- MudBlazor 8.6.0
- SQLitePCLRaw.lib.e_sqlite3 2.1.11 ?? (Deprecated)

**API Compatibility:**
- Binary Incompatible: 0
- Source Incompatible: 7 ??
- Behavioral Changes: 6 ??
- Compatible: 12,119 (99.9%)

#### Target State

- **Target Framework:** net10.0
- **Updated Packages:** 4 packages upgraded to version 10.0.2
- **Unchanged Packages:** MudBlazor 8.6.0 (already compatible)
- **Deprecated Package:** SQLitePCLRaw.lib.e_sqlite3 addressed

#### Migration Steps

##### 1. Prerequisites

**Dependencies:**
- ? Infrastructure\HockeyPool.Infrastructure.csproj must be updated to net10.0 first
- In All-At-Once strategy, both update simultaneously, but Infrastructure fixes take priority

##### 2. Framework Update

**File:** `HockeyPool\HockeyPool.csproj`

**Change Required:**
```xml
<!-- Current -->
<TargetFramework>net9.0</TargetFramework>

<!-- Updated -->
<TargetFramework>net10.0</TargetFramework>
```

##### 3. Package Updates

Update Entity Framework Core packages and address deprecated package:

| Package | Current Version | Target Version | Reason |
|---------|----------------|----------------|--------|
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore | 9.0.4 | 10.0.2 | .NET 10 compatibility |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.4 | 10.0.2 | .NET 10 compatibility |
| Microsoft.EntityFrameworkCore.Design | 9.0.4 | 10.0.2 | .NET 10 compatibility |
| Microsoft.EntityFrameworkCore.Sqlite | 9.0.4 | 10.0.2 | .NET 10 compatibility |
| MudBlazor | 8.6.0 | (no change) | Already compatible with .NET 10 |
| SQLitePCLRaw.lib.e_sqlite3 | 2.1.11 | **REMOVE** | ?? Deprecated - likely transitive dependency |

**Updated Package References:**
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore" Version="10.0.2" />
  <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.2" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.2">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.2" />
  <PackageReference Include="MudBlazor" Version="8.6.0" />
  <!-- SQLitePCLRaw.lib.e_sqlite3 removed - check if explicitly referenced or transitive -->
</ItemGroup>
```

**?? Deprecated Package Strategy:**

SQLitePCLRaw.lib.e_sqlite3 is deprecated. Recommended approach:

1. **First, check if explicitly referenced:**
   - If it appears in HockeyPool.csproj, remove the PackageReference
   - If it's a transitive dependency, no action needed

2. **Verify SQLite functionality after upgrade:**
   - Microsoft.EntityFrameworkCore.Sqlite 10.0.2 should include necessary SQLite support
   - Test database operations to ensure SQLite works correctly

3. **If issues arise:**
   - Consider SQLitePCLRaw.bundle_e_sqlite3 as replacement
   - Alternative: Switch to SQL Server (Infrastructure project already references SqlServer)

##### 4. Expected Breaking Changes

The assessment identifies **7 source incompatible APIs** requiring code changes. All are expected to be in **HockeyPool\Program.cs**.

See detailed breaking changes in the [Breaking Changes Catalog](#breaking-changes-catalog) section.

**Summary of Changes Needed:**

1. **Middleware Extension Methods Moved** (4 issues)
   - `UseMigrationsEndPoint()` - namespace/type changed
   - `AddDatabaseDeveloperPageExceptionFilter()` - namespace/type changed
   - `AddEntityFrameworkStores<T>()` - namespace/type changed
   - `UseExceptionHandler(string, bool)` - signature changed

2. **TimeSpan API Change** (1 issue)
   - `TimeSpan.FromMinutes()` - parameter type changed

3. **System.Uri Behavioral Changes** (6 issues)
   - 4 System.Uri instances with behavioral changes
   - 1 Uri.AbsoluteUri property behavioral change

##### 5. Code Modifications

**Primary File:** `HockeyPool\Program.cs`

**Expected Modifications:**

1. **Fix Middleware Registration:**
```csharp
// BEFORE (.NET 9)
using Microsoft.AspNetCore.Builder;

app.UseMigrationsEndPoint();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// AFTER (.NET 10)
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore; // Add namespace

app.UseMigrationsEndPoint(); // May need fully qualified call
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // May need fully qualified call
```

2. **Fix Identity Entity Framework Registration:**
```csharp
// BEFORE (.NET 9)
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// AFTER (.NET 10)
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Ensure correct namespace

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>(); // May require namespace fix
```

3. **Fix TimeSpan.FromMinutes (if used):**
```csharp
// BEFORE (.NET 9)
TimeSpan.FromMinutes(someIntValue)

// AFTER (.NET 10)
TimeSpan.FromMinutes((long)someIntValue) // Or use explicit long literal
```

4. **Review System.Uri Usage:**
- Locate all Uri instantiations and operations
- Test behavior to ensure no regressions
- Pay special attention to Uri.AbsoluteUri property usage

**Additional Areas to Check:**
- Any other startup configuration in Program.cs
- Blazor component code using System.Uri
- Any code using TimeSpan.FromMinutes

##### 6. Testing Strategy

**Unit Tests:**
- Run all unit tests for Blazor components
- Focus on components with API usage changes
- Verify authentication/authorization tests

**Integration Tests:**
- Test full application startup
- Verify database migrations apply correctly
- Test Entity Framework database operations
- Verify SQLite functionality (due to deprecated package)

**Manual Testing:**
- **Authentication Flow:** Login, logout, registration
- **Betting Features:** Create bets, view betting overview
- **User Management:** Admin panel, role management
- **Betting History:** View history, user predictions
- **Winners Display:** View winners page
- **Navigation:** All routes and page navigation

**Behavioral Change Testing:**
- Test all pages that construct or parse URIs
- Verify exception handling middleware works as expected
- Check any time-based calculations using TimeSpan

**Performance Testing:**
- Verify page load times haven't degraded
- Check database query performance (EF Core 10 changes)

##### 7. Validation Checklist

- [ ] Project file TargetFramework updated to net10.0
- [ ] All 4 EF Core PackageReferences updated to version 10.0.2
- [ ] SQLitePCLRaw.lib.e_sqlite3 reference removed (if explicit)
- [ ] MudBlazor 8.6.0 version confirmed (no change needed)
- [ ] `dotnet restore` completes without errors
- [ ] Project builds without errors (`dotnet build HockeyPool\HockeyPool.csproj`)
- [ ] Project builds without warnings
- [ ] All middleware extension method compilation errors fixed
- [ ] Identity registration compilation errors fixed
- [ ] TimeSpan.FromMinutes usage fixed (if applicable)
- [ ] No package dependency conflicts
- [ ] Application starts successfully
- [ ] SQLite database operations work correctly
- [ ] Authentication/authorization functional
- [ ] All Blazor pages load without errors
- [ ] Unit tests pass (if present)
- [ ] Integration tests pass (if present)
- [ ] Manual smoke testing complete
- [ ] No behavioral regressions detected

---

## Package Update Reference

### Overview

**Total Packages:** 10  
**Packages Requiring Updates:** 8  
**Compatible Packages:** 1 (MudBlazor)  
**Deprecated Packages:** 1 (SQLitePCLRaw.lib.e_sqlite3)

### Common Package Updates (Affecting Multiple Projects)

| Package | Current | Target | Projects Affected | Update Reason |
|---------|---------|--------|-------------------|---------------|
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.4 | 10.0.2 | 2 projects:<br/>• HockeyPool.csproj<br/>• HockeyPool.Infrastructure.csproj | .NET 10 compatibility |

### Infrastructure-Specific Package Updates

| Package | Current | Target | Update Reason |
|---------|---------|--------|---------------|
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.4 | 10.0.2 | .NET 10 compatibility, SQL Server database support |
| Microsoft.EntityFrameworkCore.Tools | 9.0.4 | 10.0.2 | .NET 10 compatibility, design-time EF Core tools |
| Microsoft.Extensions.Hosting.Abstractions | 9.0.4 | 10.0.2 | .NET 10 compatibility, hosting abstractions |
| Microsoft.Extensions.Identity.Stores | 9.0.4 | 10.0.2 | .NET 10 compatibility, Identity data stores |

### HockeyPool Application-Specific Package Updates

| Package | Current | Target | Update Reason |
|---------|---------|--------|---------------|
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore | 9.0.4 | 10.0.2 | .NET 10 compatibility, EF Core diagnostics middleware |
| Microsoft.EntityFrameworkCore.Design | 9.0.4 | 10.0.2 | .NET 10 compatibility, design-time EF Core tools |
| Microsoft.EntityFrameworkCore.Sqlite | 9.0.4 | 10.0.2 | .NET 10 compatibility, SQLite database provider |

### Compatible Packages (No Update Required)

| Package | Current Version | Status | Notes |
|---------|----------------|--------|-------|
| MudBlazor | 8.6.0 | ? Compatible | Already supports .NET 10, no update needed |

### Deprecated Packages Requiring Action

| Package | Current Version | Status | Recommended Action |
|---------|----------------|--------|-------------------|
| SQLitePCLRaw.lib.e_sqlite3 | 2.1.11 | ?? Deprecated | **Remove explicit reference** (if present in project file)<br/>• Likely transitive dependency of EF Core Sqlite<br/>• Microsoft.EntityFrameworkCore.Sqlite 10.0.2 should provide necessary SQLite support<br/>• **Alternative:** SQLitePCLRaw.bundle_e_sqlite3 (if needed)<br/>• **Fallback:** Switch to SQL Server (Infrastructure already configured) |

### Package Update Execution Order

**All-At-Once Strategy:** All packages updated simultaneously

**Logical Priority for Verification:**
1. **Infrastructure project packages first** (dependency of HockeyPool)
2. **HockeyPool application packages second**

**Within Each Project:**
1. Update Microsoft.AspNetCore.Identity.EntityFrameworkCore (shared foundation)
2. Update Entity Framework Core packages (database providers, tools)
3. Update Extensions packages (hosting, identity stores)
4. Address SQLitePCLRaw.lib.e_sqlite3 (remove if explicit reference)
5. Verify MudBlazor compatibility (no action needed, but confirm)

### Package Compatibility Matrix

| Package | .NET 9 Support | .NET 10 Support | Breaking Changes |
|---------|---------------|----------------|------------------|
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore | ? 9.0.4 | ? 10.0.2 | Extension method namespace changes |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | ? 9.0.4 | ? 10.0.2 | Extension method namespace changes |
| Microsoft.EntityFrameworkCore.Design | ? 9.0.4 | ? 10.0.2 | None reported |
| Microsoft.EntityFrameworkCore.Sqlite | ? 9.0.4 | ? 10.0.2 | None reported |
| Microsoft.EntityFrameworkCore.SqlServer | ? 9.0.4 | ? 10.0.2 | None reported |
| Microsoft.EntityFrameworkCore.Tools | ? 9.0.4 | ? 10.0.2 | None reported |
| Microsoft.Extensions.Hosting.Abstractions | ? 9.0.4 | ? 10.0.2 | None reported |
| Microsoft.Extensions.Identity.Stores | ? 9.0.4 | ? 10.0.2 | None reported |
| MudBlazor | ? 8.6.0 | ? 8.6.0 | None - same version |
| SQLitePCLRaw.lib.e_sqlite3 | ?? 2.1.11 (deprecated) | ? Not recommended | Deprecated - remove |

### Version Verification Commands

After package updates, verify versions with:

```bash
# List all package references
dotnet list Infrastructure\HockeyPool.Infrastructure.csproj package

dotnet list HockeyPool\HockeyPool.csproj package

# Check for outdated packages (should show none after upgrade)
dotnet list Infrastructure\HockeyPool.Infrastructure.csproj package --outdated

dotnet list HockeyPool\HockeyPool.csproj package --outdated

# Check for deprecated packages
dotnet list Infrastructure\HockeyPool.Infrastructure.csproj package --deprecated

dotnet list HockeyPool\HockeyPool.csproj package --deprecated

# Check for vulnerable packages (should show none)
dotnet list Infrastructure\HockeyPool.Infrastructure.csproj package --vulnerable

dotnet list HockeyPool\HockeyPool.csproj package --vulnerable
```

---

## Breaking Changes Catalog

### Overview

The upgrade from .NET 9 to .NET 10 introduces **13 API compatibility issues** in the HockeyPool application:

- **7 Source Incompatible APIs** - Require code changes or namespace adjustments
- **6 Behavioral Changes** - APIs function differently, require testing

### Source Incompatible APIs (Compilation Errors Expected)

#### 1. Middleware Extension Methods Namespace Changes

**Impact:** 4 APIs in 2 files - **MEDIUM SEVERITY**

##### Issue 1.1: `UseMigrationsEndPoint()` Extension Method

**File:** `HockeyPool\Configuration\HttpPipelineConfiguration.cs`  
**Line:** 8  
**Current Code:**
```csharp
app.UseMigrationsEndPoint();
```

**Issue:** Extension method type moved in .NET 10  
**Category:** Source Incompatible  
**Resolution Required:**
```csharp
// Add namespace
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;

// Method call remains the same
app.UseMigrationsEndPoint();
```

**Reference:** [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

---

##### Issue 1.2: `AddDatabaseDeveloperPageExceptionFilter()` Extension Method

**File:** `HockeyPool\Configuration\DatabaseConfiguration.cs`  
**Line:** 15  
**Current Code:**
```csharp
services.AddDatabaseDeveloperPageExceptionFilter();
```

**Issue:** Extension method type moved in .NET 10  
**Category:** Source Incompatible  
**Resolution Required:**
```csharp
// Add namespace
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;

// Method call remains the same
services.AddDatabaseDeveloperPageExceptionFilter();
```

**Reference:** [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

---

##### Issue 1.3: `AddEntityFrameworkStores<T>()` Extension Method

**File:** `HockeyPool\Configuration\AuthenticationConfiguration.cs`  
**Line:** 35 (part of larger Identity configuration block)  
**Current Code:**
```csharp
services.AddIdentityCore<ApplicationUser>(options =>
{
    // ... options configuration ...
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()  // <- Issue here
    .AddSignInManager()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<AppErrorDescriber>();
```

**Issue:** Extension method type moved in .NET 10  
**Category:** Source Incompatible  
**Resolution Required:**
```csharp
// Add namespace at top of file
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// Method call remains the same
.AddEntityFrameworkStores<ApplicationDbContext>()
```

**Reference:** [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

---

#### 2. Exception Handler Middleware Behavioral Change

**File:** `HockeyPool\Configuration\HttpPipelineConfiguration.cs`  
**Line:** 12  
**Current Code:**
```csharp
app.UseExceptionHandler("/Error", createScopeForErrors: true);
```

**Issue:** Behavioral change in `UseExceptionHandler` method in .NET 10  
**Category:** Behavioral Change (but flagged as potential breaking)  
**Impact:** Exception handling behavior may differ  

**Resolution Required:**
- Code should compile without changes
- **Testing required:** Verify exception handling works as expected
- Review exception pages and error logging
- Consider reviewing middleware order

**Reference:** [ASP.NET Core middleware documentation](https://learn.microsoft.com/aspnet/core/fundamentals/error-handling)

---

#### 3. TimeSpan API Signature Change

**File:** `HockeyPool\Components\Account\IdentityRevalidatingAuthenticationStateProvider.cs`  
**Line:** 16  
**Current Code:**
```csharp
TimeSpan.FromMinutes(30)
```

**Issue:** `TimeSpan.FromMinutes()` parameter changed from `double` to `long` in .NET 10  
**Category:** Source Incompatible  
**Impact:** May cause compilation errors if integer literals aren't inferred correctly

**Resolution Required:**
```csharp
// OPTION 1: Explicit long literal
TimeSpan.FromMinutes(30L)

// OPTION 2: Cast to long (if variable)
TimeSpan.FromMinutes((long)minutesValue)

// For literal 30, likely will compile without changes
// but explicit long is safer
```

**Reference:** [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

---

### Behavioral Changes (Runtime Testing Required)

#### 4. System.Uri Behavioral Changes

**Impact:** 4 instances across 2 files - **LOW-MEDIUM SEVERITY**

##### Instance 4.1: Uri.AbsoluteUri Property

**File:** `HockeyPool\Components\Account\Pages\Register.razor.iQr9KUNlIjFc-PNQ.ide.g.cs`  
**Line:** 521  
**Current Code:**
```csharp
var callbackUrl = NavigationManager.GetUriWithQueryParameters(
    NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
    new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = ReturnUrl });
```

**Issue:** `System.Uri` and `Uri.AbsoluteUri` have behavioral changes in .NET 10  
**Category:** Behavioral Change  
**Impact:** 
- URI encoding/decoding may differ
- Query parameter handling may change
- Fragment handling may change

**Resolution Required:**
- Code should compile without changes
- **Testing required:** 
  - Test email confirmation flow
  - Verify callback URL is constructed correctly
  - Check that query parameters (userId, code, returnUrl) work properly
  - Test with special characters in return URLs

---

##### Instance 4.2-4.4: Uri Operations in IdentityRedirectManager

**File:** `HockeyPool\Components\Account\IdentityRedirectManager.cs`  
**Lines:** 23, 37, 49  
**Current Code:**

```csharp
// Line 23
if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
{
    uri = navigationManager.ToBaseRelativePath(uri);
}

// Line 37
var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);

// Line 49
navigationManager.ToAbsoluteUri(navigationManager.Uri)
```

**Issue:** `System.Uri` behavioral changes in .NET 10  
**Category:** Behavioral Change  
**Impact:**
- URI validation may behave differently
- URI parsing/manipulation may change
- Base relative path conversions may differ

**Resolution Required:**
- Code should compile without changes
- **Testing required:**
  - Test all redirect scenarios (login, logout, access denied)
  - Verify relative/absolute URI conversions
  - Test navigation with query strings
  - Test navigation with return URLs

---

### Deprecated Package

#### SQLitePCLRaw.lib.e_sqlite3 2.1.11

**File:** `HockeyPool\HockeyPool.csproj`  
**Status:** ⚠️ Deprecated  
**Recommendation:** Package ID changed to `SourceGear.sqlite3`

**Resolution Options:**

**Option A: Remove Explicit Reference (Recommended)**
1. Check if package is explicitly referenced in HockeyPool.csproj
2. If yes, remove the PackageReference
3. Let Microsoft.EntityFrameworkCore.Sqlite 10.0.2 bring in necessary SQLite dependencies
4. Test SQLite functionality

**Option B: Replace with Non-Deprecated Package**
```xml
<!-- Remove -->
<PackageReference Include="SQLitePCLRaw.lib.e_sqlite3" Version="2.1.11" />

<!-- Add -->
<PackageReference Include="SQLitePCLRaw.bundle_e_sqlite3" Version="latest" />
```

**Option C: Use New Package ID**
```xml
<!-- As recommended by deprecation notice -->
<PackageReference Include="SourceGear.sqlite3" Version="2.1.11" />
```

**Recommended Approach:** Try Option A first (remove reference), as EF Core Sqlite should handle dependencies automatically.

**Reference:** [NuGet Package Deprecation](https://go.microsoft.com/fwlink/?linkid=2262531)

---

### Summary of Required Actions

| File | Line | Issue | Action Required | Priority |
|------|------|-------|----------------|----------|
| HttpPipelineConfiguration.cs | 8 | UseMigrationsEndPoint namespace | Add `using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;` | HIGH |
| HttpPipelineConfiguration.cs | 12 | UseExceptionHandler behavior change | Test exception handling | MEDIUM |
| DatabaseConfiguration.cs | 15 | AddDatabaseDeveloperPageExceptionFilter namespace | Add `using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;` | HIGH |
| AuthenticationConfiguration.cs | 35 | AddEntityFrameworkStores namespace | Add `using Microsoft.AspNetCore.Identity.EntityFrameworkCore;` | HIGH |
| IdentityRevalidatingAuthenticationStateProvider.cs | 16 | TimeSpan.FromMinutes signature | Change to `30L` or cast | MEDIUM |
| Register.razor (generated) | 521 | Uri.AbsoluteUri behavior | Test email confirmation flow | MEDIUM |
| IdentityRedirectManager.cs | 23, 37, 49 | System.Uri behavior changes | Test all redirect scenarios | MEDIUM |
| HockeyPool.csproj | N/A | SQLitePCLRaw.lib.e_sqlite3 deprecated | Remove reference or replace | MEDIUM |

### Testing Priorities

**Critical Testing Areas:**
1. ✅ **Application Startup** - Verify all middleware configures correctly
2. ✅ **Authentication Flow** - Login, logout, registration, email confirmation
3. ✅ **Redirect Scenarios** - Access denied, return URLs, navigation
4. ✅ **Database Operations** - SQLite connectivity and EF Core queries
5. ✅ **Exception Handling** - Verify error pages display correctly

### Breaking Changes Documentation

**Microsoft Official Documentation:**
- [Breaking changes in .NET 10](https://go.microsoft.com/fwlink/?linkid=2262679)
- [ASP.NET Core 10.0 breaking changes](https://go.microsoft.com/fwlink/?linkid=2262530)
- [EF Core 10.0 breaking changes](https://learn.microsoft.com/ef/core/what-is-new/ef-core-10.0/breaking-changes)

---

## Risk Management

### High-Level Risk Assessment

**Overall Risk Level: ?? LOW**

The upgrade presents minimal risk due to:
- Small solution size (2 projects, 3,288 LOC)
- Minimal code impact (13+ LOC, 0.4% of codebase)
- High API compatibility (99.9%)
- No security vulnerabilities
- Clear package upgrade path
- Simple dependency structure

### Risk Factors by Category

#### 1. **Deprecated Package Risk** - ?? MEDIUM

**Risk:** SQLitePCLRaw.lib.e_sqlite3 2.1.11 is deprecated

**Impact:**
- May not receive security updates
- May have compatibility issues with .NET 10
- Could be removed from NuGet in future

**Mitigation:**
- Evaluate if package is still needed (may be transitive dependency)
- Check if Entity Framework Core 10.0.2 includes necessary SQLite support
- If needed, find replacement package or upgrade to non-deprecated version
- Test SQLite functionality thoroughly after upgrade

**Contingency:**
- If issues arise, consider migrating to SQL Server (already referenced in Infrastructure project)
- Evaluate SqlitePCLRaw.bundle_e_sqlite3 as alternative

#### 2. **API Compatibility Risk** - ?? LOW

**Risk:** 7 source incompatible APIs, 6 behavioral changes

**Impact:**
- Compilation errors requiring code changes (source incompatible)
- Runtime behavior differences requiring testing (behavioral changes)

**Specific API Issues:**
- Extension methods moved to different types (middleware registration)
- System.Uri behavioral changes
- TimeSpan.FromMinutes signature change

**Mitigation:**
- Assessment provides specific file locations for all issues
- Breaking changes are well-documented by Microsoft
- Most issues are in Program.cs startup configuration (centralized)
- Small codebase makes verification straightforward

**Contingency:**
- All API issues have known fixes from .NET 10 migration documentation
- If specific API unavailable, alternative approaches exist (e.g., different middleware registration)

#### 3. **Testing Coverage Risk** - ?? MEDIUM

**Risk:** Unknown test coverage level

**Impact:**
- May not catch behavioral regressions
- Runtime issues may surface in production

**Mitigation:**
- Perform comprehensive manual smoke testing
- Test all critical user workflows (betting, user management, winners, history)
- Special attention to System.Uri usage (4 instances with behavioral changes)
- Test authentication/authorization (ASP.NET Identity affected)
- Test database operations (EF Core upgraded)

**Contingency:**
- Maintain ability to rollback via git branch
- Deploy to staging environment first
- Monitor application logs after upgrade

#### 4. **Deployment Coordination Risk** - ?? LOW

**Risk:** All developers must upgrade simultaneously (All-At-Once strategy)

**Impact:**
- Cannot mix .NET 9 and .NET 10 development environments
- Requires coordinated team communication

**Mitigation:**
- Use dedicated upgrade branch (upgrade-to-NET10)
- Clear communication before merging to main
- Ensure all developers have .NET 10 SDK installed before merge
- Document upgrade in team communication channels

**Contingency:**
- Developers can continue working on .NET 9 main branch
- Merge upgrade branch only when team is ready

### High-Risk Changes Table

| Project | Risk Level | Description | Mitigation |
|---------|-----------|-------------|------------|
| HockeyPool.csproj | ?? MEDIUM | 7 source incompatible APIs in Program.cs | Fix middleware registration extensions; verify exception handling |
| HockeyPool.csproj | ?? MEDIUM | Deprecated SQLitePCLRaw package | Evaluate necessity; test SQLite functionality; have SQL Server fallback |
| HockeyPool.csproj | ?? LOW | 6 behavioral changes (System.Uri, TimeSpan) | Thorough testing of affected code paths |
| Infrastructure.csproj | ?? LOW | Package upgrades only, no API issues | Standard package update, low risk |

### Security Vulnerabilities

**Status:** ? No security vulnerabilities detected

The assessment found no packages with known security vulnerabilities. This is positive for the upgrade.

### Contingency Plans

#### If SQLitePCLRaw.lib.e_sqlite3 causes issues:
1. **Option A:** Remove explicit reference if it's only a transitive dependency
2. **Option B:** Replace with SQLitePCLRaw.bundle_e_sqlite3 (maintained package)
3. **Option C:** Switch to SQL Server (Infrastructure already has SqlServer package)

#### If API compilation errors are blocking:
1. **Consult .NET 10 breaking changes documentation:** https://learn.microsoft.com/en-us/dotnet/core/compatibility/10.0
2. **Check Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore alternatives** for moved extension methods
3. **Use explicit namespace imports** if extension methods not resolving

#### If behavioral changes cause runtime issues:
1. **System.Uri changes:** Review URI construction/parsing code in 4 affected locations
2. **Exception handling:** Verify UseExceptionHandler middleware behavior matches expectations
3. **TimeSpan.FromMinutes:** Verify integer vs long parameter handling

#### If solution won't build after all fixes:
1. Clean solution (`dotnet clean`)
2. Delete bin/obj folders
3. Restore packages (`dotnet restore`)
4. Rebuild (`dotnet build`)
5. Check for missing .NET 10 SDK components

#### If tests fail after upgrade:
1. Review test framework compatibility with .NET 10
2. Update test packages if needed
3. Investigate behavioral changes in tested code paths
4. Consider test adjustments for legitimate behavior changes

### Rollback Strategy

**Git-Based Rollback:**
1. Upgrade is performed on dedicated branch (upgrade-to-NET10)
2. Original code remains intact on master branch
3. Can abandon upgrade branch if blocking issues found
4. Can cherry-pick individual fixes if needed

**Partial Rollback Not Applicable:**
- All-At-Once strategy means no partial rollback option
- Either complete upgrade or full rollback
- This is acceptable given low risk profile

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

The All-At-Once strategy requires comprehensive testing after the atomic upgrade completes.

### Phase 0: Pre-Upgrade Validation

**Before starting upgrade:**

- [ ] Verify current application works correctly on .NET 9
- [ ] Run existing tests (if any) to establish baseline
- [ ] Document current application behavior
- [ ] Verify SQLite database is accessible and functional
- [ ] Create backup of database (if production data exists)

### Phase 1: Build Validation

**After project files and packages updated, before code fixes:**

**Expected Result:** Compilation errors in specific files

**Actions:**
1. Run `dotnet restore` on solution
2. Run `dotnet build` on solution
3. Document all compilation errors
4. Verify errors match expected breaking changes

**Expected Compilation Errors:**
- HttpPipelineConfiguration.cs - UseMigrationsEndPoint namespace
- DatabaseConfiguration.cs - AddDatabaseDeveloperPageExceptionFilter namespace
- AuthenticationConfiguration.cs - AddEntityFrameworkStores namespace
- Possibly IdentityRevalidatingAuthenticationStateProvider.cs - TimeSpan.FromMinutes

**Unexpected Errors:** 
- If other errors appear, investigate before proceeding
- May indicate additional breaking changes not covered in assessment

### Phase 2: Post-Fix Build Validation

**After applying code fixes:**

**Expected Result:** Clean build with 0 errors, 0 warnings

**Validation Steps:**
1. Clean solution: `dotnet clean`
2. Restore packages: `dotnet restore`
3. Build solution: `dotnet build`
4. Verify: ✅ 0 errors, ✅ 0 warnings
5. Check for obsolete API warnings

**Checklist:**
- [ ] Infrastructure project builds successfully
- [ ] HockeyPool project builds successfully
- [ ] No compilation errors
- [ ] No compilation warnings
- [ ] All package dependencies resolved
- [ ] No deprecated package warnings (SQLitePCLRaw removed/replaced)

### Phase 3: Unit Testing (If Present)

**Actions:**
```bash
# Run all unit tests
dotnet test HockeyPool.sln --no-build

# Run with detailed output
dotnet test HockeyPool.sln --no-build --verbosity normal

# Generate test report
dotnet test HockeyPool.sln --no-build --logger "console;verbosity=detailed"
```

**Expected Result:** All existing unit tests pass

**If Tests Fail:**
- Analyze failure reasons (legitimate behavior change vs. test issues)
- Update tests if behavioral changes are expected and acceptable
- Fix code if behavior changes are regressions

**Test Coverage Focus:**
- Authentication/authorization tests
- Database access tests (EF Core)
- Business logic tests
- Component tests (Blazor)

### Phase 4: Integration Testing

**Application Startup:**
- [ ] Application starts without errors
- [ ] No runtime exceptions during startup
- [ ] All middleware configured correctly
- [ ] Database connection established (SQLite)
- [ ] Logging system functional

**Database Operations:**
- [ ] EF Core context initializes
- [ ] Database queries execute successfully
- [ ] CRUD operations work (Create, Read, Update, Delete)
- [ ] SQLite functionality confirmed (no issues from deprecated package removal)
- [ ] Migrations apply correctly (if using EF migrations)

**Authentication & Authorization:**
- [ ] Login page loads
- [ ] User can log in with valid credentials
- [ ] Authentication state persists correctly
- [ ] Authorization attributes work (Admin role checks)
- [ ] Logout functions correctly
- [ ] Session management works (30-minute revalidation)

### Phase 5: Manual Smoke Testing

**Critical User Workflows:**

#### Workflow 1: User Registration & Login
- [ ] Navigate to registration page
- [ ] Register new user account
- [ ] Verify email confirmation flow (callback URL construction - Uri behavioral change)
- [ ] Log in with new account
- [ ] Verify user is authenticated
- [ ] Log out successfully

#### Workflow 2: Betting Features
- [ ] Navigate to Betting Overview page
- [ ] View betting information
- [ ] Place new bet (if functionality exists)
- [ ] Verify bet data persists

#### Workflow 3: Betting History
- [ ] Navigate to Betting History page
- [ ] View user predictions history
- [ ] Verify historical data displays correctly

#### Workflow 4: Winners Page
- [ ] Navigate to Winners page
- [ ] View winners information
- [ ] Verify data loads correctly

#### Workflow 5: Admin Features (if applicable)
- [ ] Log in as admin user
- [ ] Access admin pages (AddAdmin)
- [ ] Test admin functionality
- [ ] Verify role-based authorization works

#### Workflow 6: Navigation & Routing
- [ ] Test all navigation links
- [ ] Verify routing works correctly
- [ ] Test redirect scenarios (IdentityRedirectManager - Uri behavioral changes)
- [ ] Test return URL handling
- [ ] Test access denied redirects

### Phase 6: Behavioral Change Specific Testing

**Critical: System.Uri Behavioral Changes**

**Test Areas:**
1. **Email Confirmation Callbacks** (Register.razor)
   - Register user
   - Check confirmation email (if sent)
   - Click confirmation link
   - Verify URL parameters (userId, code, returnUrl) parse correctly
   - Test with special characters in return URLs

2. **Identity Redirect Manager** (IdentityRedirectManager.cs)
   - Test login redirects with return URLs
   - Test logout redirects
   - Test access denied redirects
   - Test relative URI validation
   - Test absolute URI construction
   - Test query string handling

**Test Cases for Uri Behavior:**
```
Test Case 1: Simple redirect
- Navigate to protected page while logged out
- Should redirect to login with return URL
- After login, should redirect back to original page

Test Case 2: Redirect with query parameters
- Navigate to: /admin?param=value
- Should preserve query parameters through authentication flow

Test Case 3: Redirect with special characters
- Return URL with: spaces, &, =, /, ?
- Verify proper encoding/decoding

Test Case 4: Relative vs absolute URIs
- Test relative paths: /Account/Login
- Test absolute paths: http://localhost:5000/Account/Login
- Verify both handled correctly
```

**Exception Handling Testing:**
- [ ] Trigger application exception (e.g., null reference)
- [ ] Verify error page displays (/Error route)
- [ ] Check error is logged properly
- [ ] Verify `createScopeForErrors: true` works as expected

**TimeSpan Testing:**
- [ ] User authentication session revalidation (30 minutes)
- [ ] Log in and verify session doesn't expire prematurely
- [ ] Wait 30+ minutes (or adjust for testing) and verify revalidation occurs

### Phase 7: Performance & Quality Validation

**Performance Checks:**
- [ ] Page load times comparable to .NET 9 version
- [ ] Database query performance acceptable
- [ ] No memory leaks during extended testing
- [ ] Application responsive under normal load

**Quality Checks:**
- [ ] No console errors in browser developer tools
- [ ] No unhandled exceptions in application logs
- [ ] No deprecation warnings in build output
- [ ] No security warnings from package analysis

**Package Validation:**
```bash
# Verify no outdated packages
dotnet list package --outdated

# Verify no deprecated packages
dotnet list package --deprecated

# Verify no vulnerable packages
dotnet list package --vulnerable
```

### Phase 8: Regression Testing

**Compare .NET 9 vs .NET 10 Behavior:**
- [ ] All features that worked in .NET 9 work in .NET 10
- [ ] No new bugs introduced
- [ ] UI rendering identical
- [ ] Data integrity maintained
- [ ] No performance degradation

### Testing Sign-Off Criteria

**Before considering upgrade complete:**
- ✅ All compilation errors fixed
- ✅ Solution builds with 0 warnings
- ✅ All unit tests pass (if present)
- ✅ Application starts without errors
- ✅ All critical workflows tested successfully
- ✅ All behavioral change areas tested
- ✅ No security vulnerabilities
- ✅ No deprecated packages
- ✅ Performance acceptable
- ✅ No regression detected

### Testing Responsibility Matrix

| Testing Phase | Primary Owner | Verification Required |
|--------------|---------------|----------------------|
| Build Validation | Developer | Build output logs |
| Unit Testing | Developer | Test results report |
| Integration Testing | Developer/QA | Functional test checklist |
| Smoke Testing | Developer/QA | Manual test checklist |
| Behavioral Testing | Developer | Specific test scenarios |
| Performance Testing | Developer/QA | Performance metrics |
| Regression Testing | QA | Comparison report |

### Test Environment Requirements

**Development Environment:**
- .NET 10 SDK installed
- Visual Studio or VS Code with .NET 10 support
- SQLite database accessible
- Test data available

**Testing Tools:**
- Browser developer tools (F12) for console errors
- Application logging enabled
- Performance monitoring (optional: Application Insights)

### Issue Tracking During Testing

**If issues found:**
1. Document issue clearly (description, reproduction steps)
2. Classify severity (blocking, high, medium, low)
3. Determine if issue is:
   - New bug introduced by upgrade
   - Existing bug (not related to upgrade)
   - Expected behavioral change
4. Fix blocking/high severity issues before proceeding
5. Log medium/low severity issues for future resolution

---

## Complexity & Effort Assessment

### Overall Complexity Rating

**Solution Complexity: ?? LOW**

**Justification:**
- Only 2 projects
- Small codebase (3,288 LOC)
- Minimal code modifications needed (13+ LOC = 0.4%)
- Simple dependency structure (depth = 1, no cycles)
- High API compatibility (99.9%)
- Clear package upgrade path

### Per-Project Complexity

| Project | Complexity | Dependencies | Risk | Key Factors |
|---------|-----------|--------------|------|-------------|
| **Infrastructure\HockeyPool.Infrastructure.csproj** | ?? **LOW** | 0 (within solution)<br/>5 NuGet packages | ?? LOW | • No API issues<br/>• Package upgrades only<br/>• 2,829 LOC<br/>• Class library (simpler than web app) |
| **HockeyPool\HockeyPool.csproj** | ?? **MEDIUM** | 1 project<br/>5 NuGet packages | ?? MEDIUM | • 13 API issues (7 source incompatible)<br/>• Deprecated package<br/>• 459 LOC<br/>• Blazor application (more complex)<br/>• Startup configuration changes |

### Phase Complexity Assessment

**Phase 1: Atomic Upgrade**

**Complexity Rating: ?? MEDIUM**

**Operations:**
1. **Update project files** - ?? LOW (simple XML property change)
2. **Update packages** - ?? LOW (all have clear versions)
3. **Address deprecated package** - ?? MEDIUM (requires investigation)
4. **Fix compilation errors** - ?? MEDIUM (7 source incompatible APIs)
5. **Verify build** - ?? LOW (standard build verification)

**Key Complexity Drivers:**
- Source incompatible APIs require code changes (not just recompilation)
- Middleware extension methods moved to different types
- Deprecated package may need replacement strategy
- Behavioral changes require understanding of impact

**Mitigating Factors:**
- All API issues located in single file (Program.cs)
- Assessment provides exact file locations
- Microsoft documentation covers all breaking changes
- Small codebase makes verification quick

**Phase 2: Testing & Validation**

**Complexity Rating: ?? LOW**

**Operations:**
1. **Run tests** - ?? LOW (automated)
2. **Manual smoke testing** - ?? LOW (small application)
3. **Verify behavioral changes** - ?? MEDIUM (requires careful testing)

### Resource Requirements

#### Skill Levels Required

**Primary Developer:**
- ? C# / .NET development experience
- ? ASP.NET Core / Blazor knowledge
- ? Entity Framework Core familiarity
- ? Understanding of NuGet package management
- ?? Familiarity with .NET breaking changes (helpful but not required)

**Complexity Level:** Suitable for intermediate-level .NET developer

**Team Coordination:** Minimal
- Single developer can complete upgrade
- All-At-Once means no complex handoffs
- Team notification needed before merging

#### Parallel Execution Capacity

**Not Applicable** - All-At-Once strategy performs atomic upgrade

**Sequential Operations:**
1. Project/package updates (atomic)
2. Build and fix errors (iterative until 0 errors)
3. Testing (can parallelize manual testing across features)

### Dependency Ordering Complexity

**Complexity: ?? LOW**

**Dependency Structure:**
```
Infrastructure (leaf) ? HockeyPool (root)
```

**Ordering Considerations:**
- Simplest possible structure (single dependency chain)
- No coordination between multiple branches
- No risk of circular dependencies
- Clear error resolution order (Infrastructure first)

### Effort Estimation Approach

**Note:** This assessment uses **relative complexity ratings** (Low/Medium/High), not time estimates. The actual duration depends on:
- Developer experience with .NET upgrades
- Familiarity with the codebase
- Testing thoroughness required
- Unexpected issues encountered

**Relative Effort by Phase:**

| Phase | Relative Effort | Key Activities |
|-------|----------------|----------------|
| **Prerequisites** | Minimal | Verify SDK, check global.json |
| **Atomic Upgrade** | Low-Medium | Update files, packages, fix 7 API issues |
| **Testing & Validation** | Low | Run tests, smoke test application |

**Complexity Factors Affecting Effort:**
- ? **Reduces Effort:** Small codebase, clear package path, high compatibility
- ?? **Increases Effort:** API changes require code modifications, deprecated package investigation
- ?? **Unknown Factor:** Current test coverage level

### Code Change Hotspots

**Primary Change Locations** (based on assessment):

1. **HockeyPool\Program.cs** - 7 source incompatible API issues
   - Middleware registration extension methods
   - Exception handling configuration
   - Database developer page exception filter
   - Identity Entity Framework stores registration

2. **Project Files** - Both projects
   - TargetFramework property changes
   - PackageReference version updates

3. **Unknown locations** - 6 behavioral changes
   - System.Uri usage (4 instances) - assessment doesn't specify exact files
   - TimeSpan.FromMinutes usage (1 instance)

**Investigation Required:**
- Locate all System.Uri usage across solution
- Locate TimeSpan.FromMinutes usage
- Verify SQLitePCLRaw.lib.e_sqlite3 usage

---

## Source Control Strategy

### Branching Strategy

**Main Branch:** `master`  
**Upgrade Branch:** `upgrade-to-NET10`

**Branch Purpose:**
- **master:** Stable .NET 9 code, production-ready
- **upgrade-to-NET10:** Isolated workspace for .NET 10 upgrade work

**Branch Workflow:**
```
master (net9.0)
  └── upgrade-to-NET10 (upgrade work happens here)
       └── (after testing complete) → merge back to master
```

### Branch Creation

**Already created:** The upgrade branch should be created at the start of the planning process.

**Verification:**
```bash
# Check current branch
git branch

# Should show: * upgrade-to-NET10
```

**If not on upgrade branch:**
```bash
# Create and switch to upgrade branch
git checkout -b upgrade-to-NET10
```

### Commit Strategy

**All-At-Once Approach: Single Atomic Commit Preferred**

Given the All-At-Once strategy, the ideal approach is **one comprehensive commit** containing all upgrade changes:

**Recommended Commit Structure:**

**Single Commit (Preferred):**
```bash
# After all changes complete and tested
git add .
git commit -m "Upgrade solution from .NET 9 to .NET 10

- Update both projects to target net10.0
- Upgrade all Microsoft EF Core packages to 10.0.2
- Remove deprecated SQLitePCLRaw.lib.e_sqlite3 package
- Fix middleware extension method namespaces (HttpPipelineConfiguration, DatabaseConfiguration)
- Fix Identity extension method namespace (AuthenticationConfiguration)
- Fix TimeSpan.FromMinutes signature (IdentityRevalidatingAuthenticationStateProvider)
- Test all behavioral changes (System.Uri usage, exception handling)
- Verify all tests pass and application functions correctly

Closes #[issue-number] (if tracked)"
```

**Alternative: Checkpoint Commits (If Preferred)**

If you prefer incremental commits for safety:

**Commit 1: Project File Updates**
```bash
git add Infrastructure/HockeyPool.Infrastructure.csproj HockeyPool/HockeyPool.csproj
git commit -m "Update target framework to net10.0 and upgrade packages

- Update TargetFramework: net9.0 → net10.0
- Upgrade Microsoft EF Core packages: 9.0.4 → 10.0.2
- Remove deprecated SQLitePCLRaw.lib.e_sqlite3 reference"
```

**Commit 2: Code Fixes**
```bash
git add HockeyPool/Configuration/
git add HockeyPool/Components/Account/IdentityRevalidatingAuthenticationStateProvider.cs
git commit -m "Fix .NET 10 API compatibility issues

- Add namespace for UseMigrationsEndPoint (HttpPipelineConfiguration)
- Add namespace for AddDatabaseDeveloperPageExceptionFilter (DatabaseConfiguration)
- Add namespace for AddEntityFrameworkStores (AuthenticationConfiguration)
- Fix TimeSpan.FromMinutes signature (IdentityRevalidatingAuthenticationStateProvider)"
```

**Commit 3: Verification** (if needed)
```bash
git add [any test updates]
git commit -m "Verify .NET 10 upgrade - all tests pass"
```

### Commit Message Guidelines

**Format:**
```
<type>: <short summary> (max 72 chars)

<detailed description>
- Bullet point for each major change
- Include file names for clarity
- Reference issues or breaking changes

<optional footer>
Closes #issue-number
Breaking Change: <description if applicable>
```

**Examples:**

**Good commit message:**
```
upgrade: Migrate solution to .NET 10 LTS

- Updated both projects from net9.0 to net10.0
- Upgraded 8 NuGet packages (EF Core 9.0.4 → 10.0.2)
- Fixed 4 namespace issues in configuration files
- Addressed TimeSpan.FromMinutes signature change
- Removed deprecated SQLitePCLRaw.lib.e_sqlite3
- All tests pass, no behavioral regressions found

Breaking changes tested:
- System.Uri behavior (email confirmation, redirects)
- Exception handling middleware
- Identity authentication flow
```

**Poor commit message:** ❌
```
updated to net10
```

### Review and Merge Process

#### Pre-Merge Checklist

**Before creating pull request:**

- [ ] All compilation errors fixed
- [ ] Solution builds with 0 warnings
- [ ] All tests pass
- [ ] Manual smoke testing complete
- [ ] Behavioral changes tested thoroughly
- [ ] No security vulnerabilities (`dotnet list package --vulnerable`)
- [ ] No deprecated packages (`dotnet list package --deprecated`)
- [ ] Documentation updated (if needed)
- [ ] Commit history clean and meaningful

#### Pull Request Creation

**PR Title:**
```
Upgrade solution from .NET 9 to .NET 10 LTS
```

**PR Description Template:**
```markdown
## Summary
Upgrade Hockey Pool solution from .NET 9 to .NET 10 (Long Term Support).

## Changes
### Projects Updated
- ✅ Infrastructure\HockeyPool.Infrastructure.csproj (net9.0 → net10.0)
- ✅ HockeyPool\HockeyPool.csproj (net9.0 → net10.0)

### Package Updates
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore 9.0.4 → 10.0.2
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 9.0.4 → 10.0.2
- Microsoft.EntityFrameworkCore.Design 9.0.4 → 10.0.2
- Microsoft.EntityFrameworkCore.Sqlite 9.0.4 → 10.0.2
- Microsoft.EntityFrameworkCore.SqlServer 9.0.4 → 10.0.2
- Microsoft.EntityFrameworkCore.Tools 9.0.4 → 10.0.2
- Microsoft.Extensions.Hosting.Abstractions 9.0.4 → 10.0.2
- Microsoft.Extensions.Identity.Stores 9.0.4 → 10.0.2
- ⚠️ Removed deprecated SQLitePCLRaw.lib.e_sqlite3 2.1.11

### Code Changes
Fixed 7 source incompatible API issues:
- `HttpPipelineConfiguration.cs` - Added namespace for UseMigrationsEndPoint
- `DatabaseConfiguration.cs` - Added namespace for AddDatabaseDeveloperPageExceptionFilter
- `AuthenticationConfiguration.cs` - Added namespace for AddEntityFrameworkStores
- `IdentityRevalidatingAuthenticationStateProvider.cs` - Fixed TimeSpan.FromMinutes signature

### Breaking Changes Addressed
Tested 6 behavioral changes:
- ✅ System.Uri behavior (email confirmation URLs tested)
- ✅ Exception handling middleware (error pages verified)
- ✅ Identity redirects (login/logout flows tested)
- ✅ SQLite functionality (database operations confirmed working)

## Testing Performed
- ✅ Solution builds with 0 errors, 0 warnings
- ✅ All unit tests pass (if applicable)
- ✅ Manual smoke testing complete
- ✅ Authentication flow tested
- ✅ Database operations verified
- ✅ All critical workflows functional
- ✅ No security vulnerabilities detected

## Migration Guide for Team
After this PR merges:
1. Ensure .NET 10 SDK installed: `dotnet --version` (should be 10.x.x)
2. Pull latest master branch
3. Run `dotnet restore`
4. Run `dotnet build`
5. Test your feature branches - may need rebasing

## Rollback Plan
If issues found after merge:
- Revert this PR commit
- Return to master branch (still on .NET 9)

## References
- [.NET 10 Breaking Changes](https://go.microsoft.com/fwlink/?linkid=2262679)
- [Assessment Report](.github/upgrades/assessment.md)
- [Upgrade Plan](.github/upgrades/plan.md)
```

#### Code Review Requirements

**Reviewers should verify:**
- [ ] All project files correctly updated to net10.0
- [ ] All package versions correctly updated to 10.0.2
- [ ] Deprecated package removed
- [ ] Namespace additions make sense
- [ ] No unintended changes included
- [ ] Build succeeds in review environment
- [ ] Breaking changes properly addressed

**Review Focus Areas:**
1. **Configuration files** - Middleware and service registration
2. **Authentication code** - Identity configuration
3. **URI handling** - Email confirmation, redirects
4. **Database code** - EF Core operations

#### Merge Criteria

**All must be ✅ before merging:**
- [ ] PR approved by at least 1 reviewer (if team process requires)
- [ ] All CI/CD checks pass (if configured)
- [ ] No merge conflicts with master
- [ ] All checklist items completed
- [ ] No unresolved review comments

#### Merge Strategy

**Recommended: Squash and Merge**

If using checkpoint commits, squash into single commit on merge:
```
Upgrade solution from .NET 9 to .NET 10 LTS

[Full description of all changes]
```

**Alternative: Regular Merge**

If using single atomic commit, regular merge is fine:
```bash
git checkout master
git merge upgrade-to-NET10 --no-ff
```

### Post-Merge Actions

**After merge to master:**

1. **Verify master branch:**
```bash
git checkout master
git pull
dotnet restore
dotnet build
dotnet test (if applicable)
```

2. **Tag the release:**
```bash
git tag -a v1.0.0-net10 -m "Upgraded to .NET 10 LTS"
git push origin v1.0.0-net10
```

3. **Notify team:**
- Send email/Slack message about upgrade
- Include instructions for updating local environments
- Mention any new requirements (.NET 10 SDK)

4. **Update documentation:**
- Update README.md with .NET 10 requirement
- Update development setup guides
- Update CI/CD pipelines (if applicable)

5. **Clean up branch:**
```bash
# Optional: Delete upgrade branch after successful merge
git branch -d upgrade-to-NET10
git push origin --delete upgrade-to-NET10
```

### Rollback Strategy

**If issues found after merge:**

**Option 1: Revert the merge commit**
```bash
git revert -m 1 <merge-commit-hash>
git push origin master
```

**Option 2: Hard reset (if no one else pulled)**
```bash
git reset --hard <commit-before-merge>
git push origin master --force
```

**Option 3: Create hotfix from backup**
```bash
# If upgrade branch still exists
git checkout master
git reset --hard upgrade-to-NET10~1  # Go to commit before upgrade
```

### Branch Protection (Recommended)

**Configure on master branch:**
- Require pull request reviews
- Require status checks to pass
- Require branches to be up to date before merging
- Include administrators (if appropriate)

### All-At-Once Strategy Specific Considerations

**Why Single Commit Works Well:**
- All changes are interdependent (framework + packages + code fixes)
- No intermediate working state (either full .NET 9 or full .NET 10)
- Easier to revert if needed (single commit to revert)
- Clear atomic change in history
- Simpler for team to understand ("the .NET 10 upgrade commit")

**When Checkpoint Commits Make Sense:**
- Very large codebase (not applicable here - only 3,288 LOC)
- Long-running upgrade process (taking multiple days)
- Multiple developers working on upgrade (unlikely for 2-project solution)
- Need to share progress incrementally

**For this solution:** Single atomic commit is recommended due to small size and All-At-Once strategy.

---

## Success Criteria

### Technical Success Criteria

The .NET 10 upgrade is technically successful when **ALL** of the following criteria are met:

#### 1. Framework Migration Complete ✅

- [ ] **Infrastructure\HockeyPool.Infrastructure.csproj**
  - [ ] TargetFramework property set to `net10.0`
  - [ ] Project file is valid XML
  - [ ] No syntax errors in project file

- [ ] **HockeyPool\HockeyPool.csproj**
  - [ ] TargetFramework property set to `net10.0`
  - [ ] Project file is valid XML
  - [ ] No syntax errors in project file

#### 2. Package Updates Complete ✅

- [ ] **All 8 required packages upgraded to version 10.0.2:**
  - [ ] Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore → 10.0.2
  - [ ] Microsoft.AspNetCore.Identity.EntityFrameworkCore → 10.0.2
  - [ ] Microsoft.EntityFrameworkCore.Design → 10.0.2
  - [ ] Microsoft.EntityFrameworkCore.Sqlite → 10.0.2
  - [ ] Microsoft.EntityFrameworkCore.SqlServer → 10.0.2
  - [ ] Microsoft.EntityFrameworkCore.Tools → 10.0.2
  - [ ] Microsoft.Extensions.Hosting.Abstractions → 10.0.2
  - [ ] Microsoft.Extensions.Identity.Stores → 10.0.2

- [ ] **MudBlazor 8.6.0** remains unchanged (already compatible)

- [ ] **SQLitePCLRaw.lib.e_sqlite3** deprecated package addressed:
  - [ ] Explicit reference removed from project file (if present)
  - [ ] No deprecation warnings in build output
  - [ ] SQLite functionality verified working

#### 3. Build Success ✅

- [ ] **Clean restore:** `dotnet restore` completes without errors
- [ ] **Infrastructure project builds:** `dotnet build Infrastructure\HockeyPool.Infrastructure.csproj`
  - [ ] 0 errors
  - [ ] 0 warnings
- [ ] **HockeyPool project builds:** `dotnet build HockeyPool\HockeyPool.csproj`
  - [ ] 0 errors
  - [ ] 0 warnings
- [ ] **Full solution builds:** `dotnet build HockeyPool.sln`
  - [ ] 0 errors
  - [ ] 0 warnings
- [ ] **No package dependency conflicts**
- [ ] **No obsolete API warnings**

#### 4. Code Changes Applied ✅

All 7 source incompatible API issues fixed:

- [ ] **HttpPipelineConfiguration.cs (Line 8):**
  - [ ] Namespace added: `using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;`
  - [ ] `UseMigrationsEndPoint()` compiles successfully

- [ ] **HttpPipelineConfiguration.cs (Line 12):**
  - [ ] `UseExceptionHandler()` behavior reviewed
  - [ ] Exception handling tested

- [ ] **DatabaseConfiguration.cs (Line 15):**
  - [ ] Namespace added: `using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;`
  - [ ] `AddDatabaseDeveloperPageExceptionFilter()` compiles successfully

- [ ] **AuthenticationConfiguration.cs (Line 35):**
  - [ ] Namespace added: `using Microsoft.AspNetCore.Identity.EntityFrameworkCore;`
  - [ ] `AddEntityFrameworkStores<ApplicationDbContext>()` compiles successfully

- [ ] **IdentityRevalidatingAuthenticationStateProvider.cs (Line 16):**
  - [ ] `TimeSpan.FromMinutes()` fixed (explicit long literal or cast)
  - [ ] Compiles without warnings

- [ ] **System.Uri behavioral changes addressed:**
  - [ ] All Uri usage locations identified
  - [ ] Behavioral changes documented and understood
  - [ ] Testing completed for Uri-related functionality

#### 5. Testing Success ✅

- [ ] **Unit tests pass** (if present)
  - [ ] All existing tests run successfully
  - [ ] No test failures
  - [ ] No test skips (unless intentional)

- [ ] **Integration tests pass** (if present)
  - [ ] Database connectivity verified
  - [ ] Authentication flow working
  - [ ] All integration test scenarios pass

- [ ] **Application starts successfully**
  - [ ] No runtime exceptions during startup
  - [ ] All middleware configured correctly
  - [ ] Database connection established

- [ ] **Manual smoke testing complete:**
  - [ ] User registration works (including email confirmation URLs)
  - [ ] User login/logout works
  - [ ] Betting overview page loads
  - [ ] Betting history page loads
  - [ ] Winners page loads
  - [ ] Admin pages functional (if applicable)
  - [ ] All navigation links work
  - [ ] Redirect scenarios work (return URLs, access denied)

- [ ] **Behavioral change testing complete:**
  - [ ] System.Uri behavior verified (email confirmations, redirects)
  - [ ] Exception handling verified (error pages display)
  - [ ] TimeSpan functionality verified (session revalidation)
  - [ ] SQLite database operations verified

#### 6. Security & Quality ✅

- [ ] **No security vulnerabilities:**
  - [ ] `dotnet list package --vulnerable` returns no vulnerabilities
  - [ ] All packages from trusted sources (Microsoft, MudBlazor)

- [ ] **No deprecated packages:**
  - [ ] `dotnet list package --deprecated` returns no deprecated packages
  - [ ] SQLitePCLRaw.lib.e_sqlite3 removed or replaced

- [ ] **No outdated packages in scope:**
  - [ ] All Microsoft EF Core packages at 10.0.2
  - [ ] MudBlazor at latest compatible version

- [ ] **Code quality maintained:**
  - [ ] No new compiler warnings introduced
  - [ ] No new code analysis warnings
  - [ ] No console errors in browser developer tools

#### 7. Documentation Updated ✅

- [ ] **Assessment document:** `assessment.md` generated and accurate
- [ ] **Plan document:** `plan.md` complete and reflects actual changes
- [ ] **Commit messages:** Clear and descriptive
- [ ] **README updated** (if needed): .NET 10 requirement documented
- [ ] **Developer setup guide updated** (if exists): .NET 10 SDK requirement

---

### Quality Success Criteria

The upgrade meets quality standards when:

#### Code Quality ✅

- [ ] **All compilation warnings resolved**
  - No new warnings introduced by upgrade
  - Existing warnings (if any) remain unchanged or reduced

- [ ] **Code style consistent**
  - Namespace additions follow project conventions
  - Code formatting matches existing style
  - No unnecessary changes beyond upgrade requirements

- [ ] **No technical debt introduced**
  - No workarounds or hacks to make code compile
  - Proper resolution of breaking changes (not just suppressing errors)
  - Deprecated package properly addressed (not just ignored)

#### Test Coverage ✅

- [ ] **No reduction in test coverage**
  - All previously passing tests still pass
  - No tests disabled or skipped to make upgrade work
  - Test coverage percentage maintained or improved

- [ ] **Critical paths tested**
  - Authentication/authorization flows tested
  - Database operations tested
  - User-facing features tested
  - Admin features tested (if applicable)

#### Performance ✅

- [ ] **No performance degradation**
  - Page load times comparable to .NET 9
  - Database query performance acceptable
  - No memory leaks detected
  - Application responsive under normal load

- [ ] **Performance benchmarks** (if established):
  - Home page load: ≤ previous baseline
  - API response times: ≤ previous baseline
  - Database query times: ≤ previous baseline

---

### Process Success Criteria

The upgrade process followed best practices when:

#### All-At-Once Strategy Applied ✅

- [ ] **Atomic upgrade executed:**
  - Both projects updated simultaneously
  - All packages updated together
  - Single coordinated build and fix phase
  - No projects left on .NET 9

- [ ] **Strategy principles followed:**
  - No multi-targeting complexity
  - Single comprehensive testing phase
  - Clean dependency resolution
  - All developers upgrade simultaneously (coordinated)

#### Source Control Best Practices ✅

- [ ] **Branching strategy followed:**
  - Work performed on `upgrade-to-NET10` branch
  - Master branch (`master`) remains on .NET 9 until merge
  - Clear branch history

- [ ] **Commit strategy executed:**
  - Single atomic commit (preferred) OR
  - Logical checkpoint commits with clear messages
  - Commit messages descriptive and complete
  - No "work in progress" or unclear commits

- [ ] **Code review completed** (if process requires):
  - Pull request created with comprehensive description
  - Reviewers approved changes
  - Review comments addressed
  - No unresolved concerns

- [ ] **Merge completed successfully:**
  - upgrade-to-NET10 branch merged to master
  - No merge conflicts
  - Post-merge build succeeds
  - Tag created (optional): `v1.0.0-net10`

#### Team Communication ✅

- [ ] **Team notified of upgrade:**
  - Announcement made before starting work
  - Team informed of branch creation
  - Team notified when merge complete
  - Instructions provided for local environment updates

- [ ] **Documentation accessible:**
  - Assessment and plan documents available to team
  - Breaking changes communicated
  - Testing results shared

---

### Verification Commands

**Final verification checklist - run these commands:**

```bash
# 1. Verify .NET SDK version
dotnet --version
# Expected: 10.x.x

# 2. Verify projects target net10.0
grep -r "TargetFramework" *.csproj
# Expected: <TargetFramework>net10.0</TargetFramework> for both projects

# 3. Clean build from scratch
dotnet clean
dotnet restore
dotnet build
# Expected: Build succeeded. 0 Error(s), 0 Warning(s)

# 4. Verify package versions
dotnet list package
# Expected: All Microsoft packages at 10.0.2, MudBlazor at 8.6.0

# 5. Check for vulnerabilities
dotnet list package --vulnerable
# Expected: No vulnerable packages found

# 6. Check for deprecated packages
dotnet list package --deprecated
# Expected: No deprecated packages found

# 7. Check for outdated packages (optional - for awareness)
dotnet list package --outdated
# Expected: MudBlazor might show newer version available (optional update)

# 8. Run tests (if applicable)
dotnet test --no-build
# Expected: All tests pass

# 9. Run application
dotnet run --project HockeyPool\HockeyPool.csproj
# Expected: Application starts without errors, accessible at configured URL
```

---

### Definition of Done

**The .NET 10 upgrade is DONE when:**

✅ All Technical Success Criteria met (100%)  
✅ All Quality Success Criteria met (100%)  
✅ All Process Success Criteria met (100%)  
✅ All verification commands pass  
✅ Team has been notified and local environments updated  
✅ Changes merged to master branch  
✅ Application running successfully in .NET 10  

**At this point:** 
- The solution is fully upgraded to .NET 10 LTS
- All breaking changes addressed
- All testing complete
- Team can continue development on .NET 10
- No .NET 9 dependencies remain

---

### Sign-Off

**Required sign-offs before considering upgrade complete:**

- [ ] **Technical Lead:** Verifies all technical criteria met
- [ ] **QA/Tester:** Verifies all testing completed successfully
- [ ] **Developer(s):** Confirms all code changes correct and complete
- [ ] **Team Lead/Manager:** Approves merge to master (if required)

**Upgrade completion date:** _____________

**Signed off by:** _____________
