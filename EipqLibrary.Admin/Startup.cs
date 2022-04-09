using EipqLibrary.Admin.Extensions;
using EipqLibrary.Admin.Security;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System;
using System.Text;

namespace EipqLibrary.Admin
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
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSingleton<IAppConfig>(AppConfig);
            services.AddSingleton(AppConfig.JwtSettings);

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EipqLibrary.Admin", Version = "v1" });
            });

            services.AddRepositories();
            services.AddUnitOfWork();

            services.AddDbContext<EipqLibraryDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EipqLibraryConnectionString")));

            services.AddIdentity<AdminUser, IdentityRole>(o => o.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<EipqLibraryDbContext>()
                .AddTokenProvider<EmailConfirmationTokenProvider<AdminUser>>("emailconfirmation")
                 .AddTokenProvider<ResetTokenProvider<AdminUser>>("resetpassword");
                
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.SignIn.RequireConfirmedEmail = false;
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
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IProfessionService, ProfessionService>();
            services.AddScoped<IAdminIdentityService, AdminIdentityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminIdentityService, AdminIdentityService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAdminRefreshTokenService, AdminRefreshTokenService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBookCreationRequestService, BookCreationRequestService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();

            // Email Service
            services.AddEmailService();

            var emailSettings = new EmailSettings();
            Configuration.GetSection("EmailSettings").Bind(emailSettings);
            services.AddSingleton(emailSettings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyPolicy");
            app.UseExceptionHandler("/error");
            app.UseExceptionHandling();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EipqLibrary.Admin v1"));

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
