using JoyitasChirinos.Domain.Interfaces.Repositories;
using JoyitasChirinos.Domain.Interfaces.Services;
using JoyitasChirinos.Infrastructure.Persistence;
using JoyitasChirinos.Infrastructure.Persistence.Repositories;
using JoyitasChirinos.Infrastructure.Services.Auth;
using JoyitasChirinos.Infrastructure.Services.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JoyitasChirinos.Application.Common.Interfaces;

namespace JoyitasChirinos.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                npg => npg.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IStorageService, CloudinaryStorageService>();
         services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
         
          

        return services;
    }
}
