using HockeyPool.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class UserRepository : GenericRepository<ApplicationUser>
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<IdentityUserRole<string>>> GetAllUserRolles()
        {
            try
            {
                return _dbContext.UserRoles.ToList();
            }
            catch (Exception ex)
            {
                //why catch if you wont do anything with it?
                throw;
            }
        }

        public Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddUserRoleAsync(IdentityUserRole<string> identityUserRole)
        {
            _dbContext.UserRoles.Add(identityUserRole);
            _dbContext.SaveChanges();
        }

        public async Task RemoveUserRoleAsync(IdentityUserRole<string> identityUserRole)
        {
            _dbContext.UserRoles.Remove(identityUserRole);
            _dbContext.SaveChanges();
        }

        public async Task UpdateUserRoleAsync(IdentityUserRole<string> identityUserRole)
        {
            _dbContext.UserRoles.Update(identityUserRole);
            _dbContext.SaveChanges();
        }

        public async Task<List<Tournament>> GetListByMultipleIdsAsync(IEnumerable<int> ids)
        {
            var result = await _dbContext.Tournaments.Where(x => ids.Contains(x.Id)).ToListAsync();
            return result;
        }
    }
}
