﻿using EFund.DAL.Contexts;
using EFund.WebAPI.Utility;
using Microsoft.EntityFrameworkCore;

namespace EFund.WebAPI.Extensions;

public static class ServicesExtensions
{
    public static void MigrateDatabase(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }

    public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration,
        Action<ConfigurationServiceBuilder> configurationBuilder)
    {
        var builder = new ConfigurationServiceBuilder(services, configuration);
        configurationBuilder(builder);
        return services;
    }
}