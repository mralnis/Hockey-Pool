

using Microsoft.Extensions.DependencyInjection;
using HockeyPool.Infrastructure.Data.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace HockeyPool.Infrastructure.Data
{
    public static class DataSetup
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<TournamentRepository>();
            services.AddScoped<MatchupRepository>();
            services.AddScoped<PredictionsRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<CountryRepository>();

            return services;
        }
        public static async Task<IServiceScope> SetupHockeyPoolDB(this IHost app)
        {
            var scope = app.Services.CreateScope();
            {
                ApplicationDbContext dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                await dbContext.Database.EnsureCreatedAsync();
                try
                {
                    await dbContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    // Ignore for now
                }
                await dbContext.SeedAsync(scope.ServiceProvider);
            }

            return scope;
        }
    }
}
