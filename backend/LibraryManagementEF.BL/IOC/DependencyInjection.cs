using LibraryManagementEF.BL.Interfaces;
using LibraryManagementEF.BL.Services;
using LibraryManagementEF.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementEF.BL.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBLLDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBorrowService, BorrowService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleAssignmentService, UserService>();
            return services;
        }
    }
}