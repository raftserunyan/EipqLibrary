using EipqLibrary.API.Security;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.EmailService.Extensions;
using EipqLibrary.EmailService.Models;
using EipqLibrary.Infrastructure.Business.Services;
using EipqLibrary.Infrastructure.Data;
using EipqLibrary.Services.DTOs.MapperProfiles;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.SharedSettings;
using EipqLibrary.Shared.SharedSettings.Interfaces;
using EipqLibrary.Shared.Web.Services;
using EipqLibrary.Shared.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using EipqLibrary.API.Extensions;

namespace EipqLibrary.API
{
    public class Startup
    {
        public AppConfig AppConfig { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppConfig = new AppConfig();
            configuration.GetSection("AppConfig").Bind(AppConfig);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAppConfig>(AppConfig);
            services.AddSingleton(AppConfig.JwtSettings);

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EipqLibrary.API", Version = "v1" });
            });

            services.AddRepositories();
            services.AddUnitOfWork();

            services.AddDbContext<EipqLibraryDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EipqLibraryConnectionString")));

            services.AddIdentity<User, IdentityRole>(o => o.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<EipqLibraryDbContext>()
                 .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
                .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation")
                 .AddTokenProvider<ResetTokenProvider<User>>("resetpassword");

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = false;
                options.Tokens.PasswordResetTokenProvider = "resetpassword";
                options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            });

            // Configuring authentication JWT Token
            var key = Encoding.ASCII.GetBytes(AppConfig.JwtSettings.JwtTokenSecret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                });

            // Services
            services.AddAutoMapper(typeof(BookProfile), typeof(CategoryProfile));
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPublicIdentityService, PublicIdentityService>();
            services.AddScoped<IPublicRefreshTokenService, PublicRefreshTokenService>();
            services.AddScoped<ITokenService>(x => x.GetRequiredService<TokenService>());
            services.AddScoped<TokenService>();
            services.AddScoped<ICurrentUserService>(x => x.GetRequiredService<TokenService>());
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IProfessionService, ProfessionService>();

            // Email Service
            services.AddEmailService();

            var emailSettings = new EmailSettings();
            Configuration.GetSection("EmailSettings").Bind(emailSettings);
            services.AddSingleton(emailSettings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseDeveloperExceptionPage();
            app.UseExceptionHandler("/error");
            app.UseExceptionHandling();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EipqLibrary.API v1"));

            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
