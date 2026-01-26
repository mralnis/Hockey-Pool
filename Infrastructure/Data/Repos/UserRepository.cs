using HockeyPool.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace HockeyPool.Infrastructure.Data.Repos;

public class UserRepository : GenericRepository<ApplicationUser>
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<List<IdentityUserRole<string>>> GetAllUserRolles()
    {
        return await _dbContext.UserRoles.ToListAsync();
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task AddUserRoleAsync(IdentityUserRole<string> identityUserRole)
    {
        _dbContext.UserRoles.Add(identityUserRole);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveUserRoleAsync(IdentityUserRole<string> identityUserRole)
    {
        _dbContext.UserRoles.Remove(identityUserRole);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUserRoleAsync(IdentityUserRole<string> identityUserRole)
    {
        _dbContext.UserRoles.Update(identityUserRole);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Tournament>> GetListByMultipleIdsAsync(IEnumerable<int> ids)
    {
        var result = await _dbContext.Tournaments.Where(x => ids.Contains(x.Id)).ToListAsync();
        return result;
    }
}
