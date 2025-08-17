using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YNAER.Domain.Abstractions;
using YNAER.Domain.Entities;
using YNAER.Infrastructure.Identity;
using YNAER.Infrastructure.Persistence;
using YNAER.Infrastructure.Persistence.Repositories;

namespace YNAER.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new NpgsqlDbConnectionFactory(configuration.GetConnectionString("Database")!));

        services.AddTransient<IUserStore<ApplicationUser>, DapperUserStore>();
        services.AddTransient<IRoleStore<IdentityRole<Guid>>, DapperRoleStore>();
        services
            .AddIdentity<ApplicationUser,
                IdentityRole<Guid>>(options => { options.SignIn.RequireConfirmedAccount = false; })
            .AddUserStore<DapperUserStore>()
            .AddRoleStore<DapperRoleStore>()
            .AddDefaultTokenProviders();

        services.AddScoped<IBankAccountsRepository, BankAccountsRepository>();

        return services;
    }
}