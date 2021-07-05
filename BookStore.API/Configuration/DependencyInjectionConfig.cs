using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using BookStore.Infrastructure.Context;
using BookStore.Infrastructure.Repositories;
using LoggerService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<BookStoreDbContext>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookService, BookService>();

            services.AddScoped<ILoggerManager, LoggerManager>();

            return services;
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(u =>
            {
                u.Password.RequireDigit = true;
                u.Password.RequireLowercase = true;
                u.Password.RequireUppercase = true;
                u.Password.RequireNonAlphanumeric = false;
                u.Password.RequiredLength = 8;
                u.User.RequireUniqueEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<BookStoreDbContext>().AddDefaultTokenProviders();
        }
    }
}