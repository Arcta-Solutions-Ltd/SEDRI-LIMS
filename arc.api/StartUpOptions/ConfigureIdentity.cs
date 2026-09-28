using arc.common.Models;
using arc.domain.Security.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace arc.api.StartUpOptions;

public static class ConfigureIdentity
{
    public static IServiceCollection AddIdentity(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var serviceProvider = services.BuildServiceProvider();

        services
            .AddAuthentication("Local")
            .AddJwtBearer("Local", options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Authentication:Local:Issuer"],
                    ValidAudience = configuration["Authentication:Local:Audience"],
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Authentication:Local:Key"]))
                };
            })
            .AddMicrosoftIdentityWebApi(configuration, "Authentication:AzureAd", "AzureAd");

        services.Configure<PasswordHasherOptions>(
            options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
        );
        services.TryAddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddScoped(
            (x) =>
            {
                return new TokenInfoModel();
            }
        );

        return services;
    }
}
