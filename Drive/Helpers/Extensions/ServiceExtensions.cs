using Drive.Helpers.JwtUtils;
using Drive.Services.BaseDirService;
using Drive.Services.FileSystemService;
using Drive.Services.UserService;
using Drive.UnitOfWork;

namespace Drive.Helpers.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IFileSystemService, FileSystemService>();
            services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBaseDirService, BaseDirService>();
            return services;
        }

        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddTransient<IJwtUtils, JwtUtils.JwtUtils>();

            return services;
        }
    }
}
