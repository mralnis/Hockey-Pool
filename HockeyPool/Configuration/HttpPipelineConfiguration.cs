namespace HockeyPool.Configuration;

public static class HttpPipelineConfiguration
{
    public static WebApplication SetupHttpPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);               
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        return app;
    }
}
