using HockeyPool.Infrastructure.Data.Models;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class UserRepository(ApplicationDbContext dbContext)
    {
        public List<ApplicationUser> GetAll()
        {
           return dbContext.Users.ToList();
        } 
    }
}
