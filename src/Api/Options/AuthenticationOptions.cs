using System.Text;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Api.Options;

public static class AuthenticationPolicy
{
    public const string BackOffice = "BackOffice";
    public const string Banking = "Banking";
}

public static class AuthenticationOptions
{
    public static IServiceCollection AddAuthenticationOptions(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var jwtSettings = serviceProvider.GetService<IOptionsSnapshot<JWTSettings>>();

        if (jwtSettings is null)
        {
            throw new Exception($"ERR: Empty Service {nameof(JWTSettings)}");
        }

        var internalJWT = jwtSettings.Get(JWTSettingsSections.Internal);
        var externalJWT = jwtSettings.Get(JWTSettingsSections.External);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(
                nameof(JWTSettingsSections.Internal),
                options =>
                {
                    var key = Encoding.ASCII.GetBytes(internalJWT.Secret);

                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidIssuer = internalJWT.Issuer,
                        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                    };
                }
            )
            .AddJwtBearer(
                nameof(JWTSettingsSections.External),
                options =>
                {
                    var key = Encoding.ASCII.GetBytes(externalJWT.Secret);

                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidIssuer = externalJWT.Issuer,
                        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                    };
                }
            );

        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                AuthenticationPolicy.BackOffice,
                p =>
                    p.RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(nameof(JWTSettingsSections.Internal))
            )
            .AddPolicy(
                AuthenticationPolicy.Banking,
                p =>
                    p.RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(nameof(JWTSettingsSections.External))
            );

        return services;
    }
}
