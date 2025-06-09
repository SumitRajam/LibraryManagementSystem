using LibraryManagementEF.DAL.Data;
using LibraryManagementEF.DAL.Interfaces;
using LibraryManagementEF.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementEF.DAL.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDALDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();
            return services;
        }
    }
}