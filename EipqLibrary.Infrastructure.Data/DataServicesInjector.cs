using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EipqLibrary.Infrastructure.Data
{
    public static class DataServicesInjector
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
