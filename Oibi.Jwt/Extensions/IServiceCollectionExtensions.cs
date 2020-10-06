using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Oibi.Authentication.Extensions;
using Oibi.Jwt.Models.Configurations;
using Oibi.Jwt.Services.AuthService;

namespace Oibi.Jwt.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Ad JWT service to given <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">Automatically reads 'Jwt' section from configuration</param>
        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            // setup jwt provider service
            services.AddSingleton<IJwtProviderService, JwtProviderService>();

            // configure strongly typed settings objects
            var settingsSection = configuration.GetSection(JwtSettings.SectionName);
            services.Configure<JwtSettings>(settingsSection);

            // configure jwt authentication
            var appSettings = settingsSection.Get<JwtSettings>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false; // TODO: test with reserve proxy
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(appSettings.SecretKey),
                    ValidateIssuerSigningKey = appSettings.ValidateIssuerSigningKey,
                    ValidateIssuer = appSettings.ValidateIssuer,
                    ValidateAudience = appSettings.ValidateAudience
                };
            })
            ;

            return services;
        }
    }
}