using EipqLibrary.API.Security;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Infrastructure.Business.Services;
using EipqLibrary.Infrastructure.Data;
using EipqLibrary.Services.DTOs.MapperProfiles;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace EipqLibrary.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

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
                 .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
                .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation")
                 .AddTokenProvider<ResetTokenProvider<User>>("resetpassword");

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.PasswordResetTokenProvider = "resetpassword";
                options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            });

            // Services
            services.AddAutoMapper(typeof(BookProfile), typeof(CategoryProfile));
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPublicIdentityService, PublicIdentityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EipqLibrary.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
