using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Services;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Services;

namespace Microservicio.Vuelos.Api.Extensions;

public static class AeropuertoServiceExtensions
{
    public static IServiceCollection AddAeropuertosModule(this IServiceCollection services)
    {
        services.AddScoped<IAeropuertoDataService, AeropuertoDataService>();
        services.AddScoped<IAeropuertoService, AeropuertoService>();
        return services;
    }
}
