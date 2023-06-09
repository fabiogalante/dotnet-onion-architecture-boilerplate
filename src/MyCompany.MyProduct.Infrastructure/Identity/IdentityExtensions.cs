﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Application.Abstractions.Identity;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal static class IdentityExtensions
{
    internal static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        ConfigureScopedUserService(services);
        ConfigureIdentityCore(services);
        ConfigureIdentityOptions(services);

        return services;
    }

    private static void ConfigureScopedUserService(IServiceCollection services) =>
        services.AddScoped<IUserService, UserService>();

    private static void ConfigureIdentityCore(IServiceCollection services)
    {
        services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
        services.AddScoped<RoleManager<ApplicationRole>, RoleManager<ApplicationRole>>();
    }

    private static void ConfigureIdentityOptions(IServiceCollection services)
    {
        ConfigureIdentityPasswordOptions(services);
        ConfigureIdentityLockoutOptions(services);
        ConfigureIdentityTwoFactorOptions(services);
    }

    private static void ConfigureIdentityPasswordOptions(IServiceCollection services) =>
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredUniqueChars = 4;
        });

    private static void ConfigureIdentityLockoutOptions(IServiceCollection services) =>
        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;
        });

    private static void ConfigureIdentityTwoFactorOptions(IServiceCollection services) =>
        services.Configure<IdentityOptions>(options =>
        {
            options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
        });
}