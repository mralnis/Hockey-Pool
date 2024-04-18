using Microsoft.EntityFrameworkCore;

namespace HockeyPool.Configuration
{
    public static class SetupDB
    {
        public static IServiceCollection AddHockeyPoolDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite("Data Source=databse.dat");

               // options.UseSqlServer(connectionString);
            });

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddRepositories();

            return services;
        }
    }
}
