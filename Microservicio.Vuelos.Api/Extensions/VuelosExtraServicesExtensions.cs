using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Services;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Services;

namespace Microservicio.Vuelos.Api.Extensions;

public static class VuelosExtraServicesExtensions
{
    public static IServiceCollection AddVuelosExtraModules(this IServiceCollection services)
    {
        services.AddScoped<IPasajeroDataService, PasajeroDataService>();
        services.AddScoped<IPasajeroService, PasajeroService>();

        services.AddScoped<IBoletoDataService, BoletoDataService>();
        services.AddScoped<IBoletoService, BoletoService>();

        services.AddScoped<IEquipajeDataService, EquipajeDataService>();
        services.AddScoped<IEquipajeService, EquipajeService>();
        
        return services;
    }
}
