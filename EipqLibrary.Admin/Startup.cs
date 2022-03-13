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
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAppConfig>(AppConfig);
            services.AddSingleton(AppConfig.JwtSettings);

            services.AddControllers();
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
                 .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.SignIn.RequireConfirmedEmail = true;
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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IProfessionService, ProfessionService>();

            // Email Service
            services.AddEmailService();

            var emailSettings = new EmailSettings();
            Configuration.GetSection("EmailSettings").Bind(emailSettings);
            services.AddSingleton(emailSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
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
