using HockeyPool.Infrastructure.Data.Models;

namespace HockeyPool.Infrastructure.Data.Repos
{
    public class CountryRepository : Repository<Country>
    {
        public CountryRepository(ApplicationDbContext dbContext) : base(dbContext) { }
        public string GetCountryFlagCode(int id)
        {
           return _dbContext.Countries.FirstOrDefault(_=> _.Id == id)?.FlagCode;
        }
    }
}
