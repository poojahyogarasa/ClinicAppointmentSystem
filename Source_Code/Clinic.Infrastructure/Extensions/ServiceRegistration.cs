using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Clinic.Infrastructure.Data;
using Clinic.Domain.Interfaces;
using Clinic.Infrastructure.Services;

namespace Clinic.Infrastructure.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string dbPath)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlite($"Data Source={dbPath}"));

        // Register service implementations for Domain interfaces
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMasterDataService, MasterDataService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        return services;
    }
}
