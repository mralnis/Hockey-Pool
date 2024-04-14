using HockeyPool.Infrastructure.Data.Models;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class UserRepository : Repository<ApplicationUser>
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
