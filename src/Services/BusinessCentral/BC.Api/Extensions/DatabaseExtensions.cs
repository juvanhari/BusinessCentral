using BC.Api.Data;

namespace BC.Api.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task IntialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
            await SeedAsync(dbContext);

        }
        private static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedProductAsync(context);
        }

        private static async Task SeedProductAsync(ApplicationDbContext context)
        {
            if (!await context.Products.AnyAsync())
            {
                await context.Products.AddRangeAsync(InitialData.Products);
                await context.SaveChangesAsync();
            }
        }

    }
}
