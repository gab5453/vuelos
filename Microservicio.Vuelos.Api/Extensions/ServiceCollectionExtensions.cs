using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataAccess.Repositories;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Services;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Vuelos.Api.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra DbContext, capa DataManagement y capa Business para Servicio / TipoServicio.
    /// </summary>
    public static IServiceCollection AddVuelosApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Falta ConnectionStrings:Default en configuración.");

        services.AddDbContext<VuelosDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<VuelosDbContext>());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Registro de repositorios (Opcional si se usa IUnitOfWork puro, pero útil para inyección directa en servicios)
        services.AddScoped<IVueloRepository, VueloRepository>();
        services.AddScoped<IEscalaRepository, EscalaRepository>();
        services.AddScoped<IAsientoRepository, AsientoRepository>();
        services.AddScoped<IReservaRepository, ReservaRepository>();
        services.AddScoped<IBoletoRepository, BoletoRepository>();
        services.AddScoped<IEquipajeRepository, EquipajeRepository>();
        services.AddScoped<IPasajeroRepository, PasajeroRepository>();
        services.AddScoped<IPaisRepository, PaisRepository>();
        services.AddScoped<ICiudadRepository, CiudadRepository>();
        services.AddScoped<IAeropuertoRepository, AeropuertoRepository>();

        services.AddScoped<IVueloDataService, VueloDataService>();
        services.AddScoped<IVueloService, VueloService>();
        services.AddScoped<IReservaDataService, ReservaDataService>();
        services.AddScoped<IReservaService, ReservaService>();
        return services;
    }
}
