namespace HockeyPool.Infrastructure.Data.Repos
{
    public class CountryRepository(ApplicationDbContext dbContext)
    {
        public string GetCountryFlagCode(int id)
        {
           return dbContext.Countries.FirstOrDefault(_=> _.Id == id).FlagCode;
        } 
    }
}
