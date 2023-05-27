namespace LibraryAPI;

using LibraryData.Data;
using Microsoft.EntityFrameworkCore;

public static class Bootstrapper
{
    public static async Task MigrateAsync(this WebApplication app)
    {
        var provider = app.Services.CreateScope();
        var context = provider.ServiceProvider.GetRequiredService<LibraryContext>();

        await context.Database.MigrateAsync();
    }
}