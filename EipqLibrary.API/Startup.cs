using AutoMapper;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Domain.Interfaces.EFInterfaces.Common;
using EipqLibrary.Infrastructure.Business.Services;
using EipqLibrary.Infrastructure.Data;
using EipqLibrary.Infrastructure.Data.Repositories;
using EipqLibrary.Infrastructure.Data.Repositories.Common;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EipqLibrary.Services.DTOs.MapperProfiles;

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

            // Services
            services.AddAutoMapper(typeof(BookProfile), typeof(CategoryProfile));
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
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
